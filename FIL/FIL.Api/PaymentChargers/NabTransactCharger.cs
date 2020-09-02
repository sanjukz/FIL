using FIL.Api.Events.Event.HubSpot;
using FIL.Api.Providers.Transaction;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FIL.Api.PaymentChargers
{
    public interface INabTransactCharger<INabTransactCharge, IPaymentHtmlPostResponse> : IPaymentHtmlPostCharger<INabTransactCharge, IPaymentHtmlPostResponse>
    {
        Task<IPaymentResponse> NabTransactResponseHandler(IGatewayCharge gatewayCharge);
    }

    public class NabTransactCharger : PaymentHtmlPostCharger<INabTransactCharge>, INabTransactCharger<INabTransactCharge, IPaymentHtmlPostResponse>
    {
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly IMediator _mediator;
        private readonly ITransactionStatusUpdater _transactionStatusUpdater;

        public NabTransactCharger(ILogger logger, ISettings settings,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            IMediator mediator,
            ITransactionStatusUpdater transactionStatusUpdater
            )
            : base(logger, settings)
        {
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _mediator = mediator;
            _transactionStatusUpdater = transactionStatusUpdater;
        }

        protected override async Task<IPaymentHtmlPostResponse> PaymentHtmlPostChargeGenerator(INabTransactCharge nabTransactCharge)
        {
            try
            {
                string timeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss"), txnType = "6";
                Dictionary<string, string> formFields = new Dictionary<string, string>();
                var settingKey = _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.NabTransact.MerchantId);
                var price = nabTransactCharge.Amount.ToString("0.00");
                /* if (settingKey == "XYZ0010") // NAB test bed
                 {
                     var splitprice = price.Split(".");
                     price = splitprice[0] + ".00";
                     price = Convert.ToDecimal(price).ToString("0.00");
                 }*/
                formFields.Add("EPS_MERCHANT", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.NabTransact.MerchantId));
                formFields.Add("EPS_TXNTYPE", txnType);
                formFields.Add("EPS_REFERENCEID", nabTransactCharge.TransactionId.ToString());
                formFields.Add("EPS_AMOUNT", price);
                formFields.Add("EPS_TIMESTAMP", timeStamp);

                //FingurePrint using SHA1 Hash
                formFields.Add("EPS_FINGERPRINT", GenerateSHA1Hash($"{_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.NabTransact.MerchantId)}|{_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.NabTransact.Password)}|{txnType}|{nabTransactCharge.TransactionId.ToString()}|{nabTransactCharge.Amount.ToString("0.00")}|" + timeStamp));

                formFields.Add("EPS_RESULTURL", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.NabTransact.ReturnUrl));
                formFields.Add("EPS_CALLBACKURL", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.NabTransact.ReturnUrl));
                formFields.Add("EPS_REDIRECT", "TRUE");
                formFields.Add("EPS_CURRENCY", nabTransactCharge.Currency.ToString());

                //Card details
                formFields.Add("EPS_CARDNUMBER", nabTransactCharge.PaymentCard.CardNumber);
                formFields.Add("EPS_EXPIRYMONTH", nabTransactCharge.PaymentCard.ExpiryMonth.ToString());
                formFields.Add("EPS_EXPIRYYEAR", nabTransactCharge.PaymentCard.ExpiryYear.ToString());
                formFields.Add("EPS_CCV", nabTransactCharge.PaymentCard.Cvv);
                formFields.Add("3D_XID", timeStamp + nabTransactCharge.TransactionId.ToString("000000").Substring(0, 6));
                formFields.Add("EPS_MERCHANTNUM", "22123456");
                formFields.Add("EPS_EMAILADDRESS", nabTransactCharge.User.Email.ToString());
                formFields.Add("EPS_IP", nabTransactCharge.IPAddress);

                IHtmlPostRequest htmlPostRequest = new HtmlPostRequest();
                htmlPostRequest.Method = "Post";
                htmlPostRequest.Action = $"{_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.NabTransact.PostUrl)}";
                htmlPostRequest.FormFields = formFields;

                _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                {
                    TransactionId = Convert.ToInt64(nabTransactCharge.TransactionId),
                    PaymentOptionId = nabTransactCharge.PaymentOption,
                    PaymentGatewayId = PaymentGateway.NabTransact,
                    UserCardDetailId = nabTransactCharge.UserCardDetailId,
                    RequestType = "Charge Posting",
                    Amount = price,
                    PayConfNumber = "",
                    PaymentDetail = "{\"Request\":" + Newtonsoft.Json.JsonConvert.SerializeObject(htmlPostRequest) + "}",
                });
                /* update final transcation status */
                _transactionStatusUpdater.UpdateTranscationStatus(Convert.ToInt64(nabTransactCharge.TransactionId));

                await _mediator.Publish(new TransactionEvent
                {
                    TransactionStatus = TransactionStatus.UnderPayment,
                    TransactionId = Convert.ToInt64(nabTransactCharge.TransactionId),
                    EmailId = nabTransactCharge.User.Email,
                    ZipCode = nabTransactCharge.BillingAddress.Zipcode
                });

                return GetPaymentHtmlPostResponse(htmlPostRequest, null);
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to generate html post request", ex));
                return GetPaymentHtmlPostResponse(null, ex.Message);
            }
        }

        private static string GenerateSHA1Hash(string text)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(text));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public async Task<IPaymentResponse> NabTransactResponseHandler(IGatewayCharge gatewayResponse)
        {
            NameValueCollection nvcResponseQueryString = HttpUtility.ParseQueryString(gatewayResponse.QueryString);
            try
            {
                var resultText = nvcResponseQueryString["restext"];
                var summaryCode = nvcResponseQueryString["summarycode"];
                var resultCode = nvcResponseQueryString["rescode"];
                var paymentConfirmationNumber = nvcResponseQueryString["txnid"];
                var fingerPrint = nvcResponseQueryString["fingerprint"];
                var orderId = nvcResponseQueryString["refid"];

                _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                {
                    TransactionId = Convert.ToInt64(orderId),
                    PaymentOptionId = PaymentOptions.None,
                    PaymentGatewayId = PaymentGateway.NabTransact,
                    RequestType = "Charge Recieved",
                    Amount = "",
                    PayConfNumber = paymentConfirmationNumber,
                    PaymentDetail = "{\"Response\":" + Newtonsoft.Json.JsonConvert.SerializeObject(nvcResponseQueryString.AllKeys.ToDictionary(k => k, k => nvcResponseQueryString[k])) + "}",
                });

                if (summaryCode.Equals("1"))
                {
                    if (resultCode.Equals("00") || resultCode.Equals("08") || resultCode.Equals("11"))
                    {
                        await _mediator.Publish(new TransactionEvent
                        {
                            TransactionStatus = TransactionStatus.Success,
                            TransactionId = Convert.ToInt64(orderId)
                        });
                        return GetPaymentResponse(true, PaymentGatewayError.None, PaymentGateway.NabTransact);
                    }
                    else
                    {
                        return GetPaymentResponse(false, GetPaymentGatewayErrorCode(resultCode), PaymentGateway.NabTransact);
                    }
                }
                else
                {
                    return GetPaymentResponse(false, GetPaymentGatewayErrorCode(resultCode), PaymentGateway.NabTransact);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to handle response", ex));
                return GetPaymentResponse(false, GetPaymentGatewayErrorCode(ex.Message), PaymentGateway.NabTransact);
            }
        }

        private PaymentGatewayError GetPaymentGatewayErrorCode(string errorCode)
        {
            string errorMessage = string.Empty;
            switch (errorCode)
            {
                case "01": errorMessage = "This card has been declined, please contact your card issuing bank or try using a different card. In case you do not have another card, please see https://royalshow.com.au/ticket-outlets/  for the list of outlets from where you can also purchase your tickets to the Show."; return PaymentGatewayError.ReferToCardIssuer;
                case "03": errorMessage = "Invalid Merchant"; return PaymentGatewayError.InvalidMerchantId;
                case "05": errorMessage = "Do Not Honour (Account may be overdrawn or frozen)"; return PaymentGatewayError.DoNotHonour;
                case "12": errorMessage = "Invalid Transaction"; return PaymentGatewayError.TransactionCancelled;
                case "13": errorMessage = "Invalid Amount"; return PaymentGatewayError.InvalidAmount;
                case "14": errorMessage = "Invalid Card Number"; return PaymentGatewayError.InvalidCardNumber;
                case "17": errorMessage = "Customer Cancellation"; return PaymentGatewayError.TransactionCancelledByCustomer;
                case "33": errorMessage = "Expired Card"; return PaymentGatewayError.ExpiredCard;
                case "51": errorMessage = "Insufficient Funds "; return PaymentGatewayError.InsufficientFunds;
                case "54": errorMessage = "Expired Card"; return PaymentGatewayError.ExpiredCard;
                case "55": errorMessage = "Incorrect PIN "; return PaymentGatewayError.InvalidCvv;
                case "57": errorMessage = "Trans. not Permitted to Cardholder"; return PaymentGatewayError.UnauthorizedTransaction;
                case "61": errorMessage = "Exceeds Withdrawal Amount Limits"; return PaymentGatewayError.ExceededTransactionAttemptLimit;
                case "62": errorMessage = "The card being used for purchase has been declined as it appears to restricted by the card issuing bank to certain types of transactions only. Please contact your card issuing bank to attempt to lift restrictions on types of purchases or alternatively please try using a different card. Please see https://royalshow.com.au/ticket-outlets/  for the list of outlets where you can also purchase your tickets to the Show."; return PaymentGatewayError.RestrictedCard;
                case "75": errorMessage = "Allowable PIN Tries Exceeded"; return PaymentGatewayError.ExceededTransactionAttemptLimit;
                case "93": errorMessage = "Trans Cannot be Completed"; return PaymentGatewayError.UnableToProcessTransaction;
                case "96": errorMessage = "System Malfunction"; return PaymentGatewayError.PaymentSystemError;
                case "100": errorMessage = "Invalid Transaction Amount"; return PaymentGatewayError.InvalidAmount;
                case "101": errorMessage = "Invalid Card Number"; return PaymentGatewayError.InvalidCardNumber;
                case "102": errorMessage = "Invalid Expiry Date Format"; return PaymentGatewayError.InvalidExpirationDate;
                case "104": errorMessage = "Invalid Merchant ID"; return PaymentGatewayError.InvalidMerchantId;
                case "106": errorMessage = "Card type unsupported"; return PaymentGatewayError.CardNotSupported;
                case "109": errorMessage = "Invalid credit card CVV number format"; return PaymentGatewayError.InvalidCvv;
                case "110": errorMessage = "Unable To Connect To Server"; return PaymentGatewayError.PaymentSystemError;
                case "112": errorMessage = "Transaction timed out By Client"; return PaymentGatewayError.SessionExpired;
                case "123": errorMessage = "Gateway Timeout"; return PaymentGatewayError.SessionExpired;
                case "151": errorMessage = "Invalid Currency Code"; return PaymentGatewayError.InvalidCurrency;
                case "159": errorMessage = "This card has been declined as it has been flagged in the banking system due to certain security measures that are in place to protect against misuse. Given the confidential nature of banking systems, we are unable to share them here. Please try a different card, or if you not have another card, please see https://royalshow.com.au/ticket-outlets/  for the list of outlets where you can also purchase your tickets to the Show."; return PaymentGatewayError.SuspectedFraud;
                case "160": errorMessage = "Cardholder could  not be authenticated"; return PaymentGatewayError.CardHolderAuthorizationFailed;
                case "176": errorMessage = " Merchant Not Enrolled in 3D Secure"; return PaymentGatewayError.ThreeDSecureAuthenticationFailed;
                case "179": errorMessage = "User Cancelled Payment"; return PaymentGatewayError.TransactionCancelledByCustomer;
                default: return PaymentGatewayError.Unknown;
            }
        }
    }
}