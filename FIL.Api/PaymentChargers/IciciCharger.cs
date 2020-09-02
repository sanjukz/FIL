using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using FIL.Logging.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;

namespace FIL.Api.PaymentChargers
{
    public interface IIciciCharger<IIciciCharge, IPaymentHtmlPostResponse> : IPaymentHtmlPostCharger<IIciciCharge, IPaymentHtmlPostResponse>
    {
        IPaymentResponse IciciResponseHandler(IGatewayCharge gatewayCharge);
    }

    public class IciciCharger : PaymentHtmlPostCharger<IIciciCharge>, IIciciCharger<IIciciCharge, IPaymentHtmlPostResponse>
    {
        public IciciCharger(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        protected override async Task<IPaymentHtmlPostResponse> PaymentHtmlPostChargeGenerator(IIciciCharge iciciCharge)
        {
            try
            {
                Dictionary<string, string> formFields = new Dictionary<string, string>();
                formFields.Add("virtualPaymentClientURL", "https://migs.mastercard.com.au/vpcpay");
                formFields.Add("vpc_Version", "1");
                formFields.Add("vpc_Command", "pay");
                formFields.Add("vpc_AccessCode", JsonConvert.DeserializeObject<Dictionary<string, string>>(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Icici.AccessKey))[iciciCharge.Currency.ToLower()]);
                formFields.Add("vpc_MerchTxnRef", iciciCharge.TransactionId.ToString());
                formFields.Add("vpc_Merchant", JsonConvert.DeserializeObject<Dictionary<string, string>>(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Icici.MerchantId))[iciciCharge.Currency.ToLower()]);
                formFields.Add("vpc_OrderInfo", "Transaction charge for " + iciciCharge.TransactionId.ToString());
                formFields.Add("vpc_Amount", (Convert.ToDouble(iciciCharge.Amount) * (100)).ToString());
                formFields.Add("vpc_ReturnURL", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Icici.ReturnUrl));
                formFields.Add("vpc_Locale", "en");
                formFields.Add("vpc_gateway", "ssl");
                formFields.Add("vpc_card", iciciCharge.PaymentCard.CardType.ToString());
                formFields.Add("vpc_CardNum", iciciCharge.PaymentCard.CardNumber);
                formFields.Add("vpc_CardExp", iciciCharge.PaymentCard.ExpiryYear.ToString().Substring(2, 2) + iciciCharge.PaymentCard.ExpiryMonth);
                formFields.Add("vpc_CardSecurityCode", iciciCharge.PaymentCard.Cvv);
                formFields.Add("vpc_TicketNo", "");

                IHtmlPostRequest htmlPostRequest = new HtmlPostRequest();
                htmlPostRequest.Method = "Post";
                htmlPostRequest.Action = $"{_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Icici.PostUrl)}/{JsonConvert.DeserializeObject<Dictionary<string, string>>(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Icici.AspUrl))[iciciCharge.Currency.ToLower()]}";
                htmlPostRequest.FormFields = formFields;

                return GetPaymentHtmlPostResponse(htmlPostRequest, null);
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to generate html post request", ex));
                return GetPaymentHtmlPostResponse(null, ex.Message);
            }
        }

        public IPaymentResponse IciciResponseHandler(IGatewayCharge gatewayResponse)
        {
            NameValueCollection nvcResponseQueryString = HttpUtility.ParseQueryString(gatewayResponse.QueryString);
            try
            {
                var secureHash = nvcResponseQueryString["VPC_SECUREHASH"];
                var txnResponseCode = nvcResponseQueryString["VPC_TXNRESPONSECODE"];
                var orderInfo = nvcResponseQueryString["VPC_ORDERINFO"];
                var merchTxnRef = nvcResponseQueryString["VPC_MERCHTXNREF"];
                var transactionNo = nvcResponseQueryString["VPC_TRANSACTIONNO"];
                var amount = nvcResponseQueryString["vpc_Amount"];

                txnResponseCode = txnResponseCode != null && txnResponseCode.Length > 0 ? txnResponseCode : "Unknown";
                var paymentConfirmationNumber = transactionNo != null && transactionNo.Length > 0 ? transactionNo : "Unknown";
                amount = amount != null && amount.Length > 0 ? amount : "0.00";

                if (txnResponseCode.Equals("0"))
                {
                    return GetPaymentResponse(true, PaymentGatewayError.None);
                }
                else
                {
                    return GetPaymentResponse(false, GetPaymentGatewayErrorCode(txnResponseCode));
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to process transaction", ex));
                return GetPaymentResponse(false, GetPaymentGatewayErrorCode(ex.Message));
            }
        }

        //Maps the vpc_TxnResponseCode to a relevant description.
        private PaymentGatewayError GetPaymentGatewayErrorCode(string errorCode)
        {
            string errorMessage = string.Empty;
            switch (errorCode)
            {
                case "0": errorMessage = "Transaction Successful"; return PaymentGatewayError.None;
                case "1": errorMessage = "Transaction Declined"; return PaymentGatewayError.TransactionDeclined;
                case "2": errorMessage = "Bank Declined Transaction"; return PaymentGatewayError.TransactionDeclined;
                case "3": errorMessage = "No Reply from Bank"; return PaymentGatewayError.NoResonseFromBank;
                case "4": errorMessage = "Expired Card"; return PaymentGatewayError.ExpiredCard;
                case "5": errorMessage = "Insufficient Funds"; return PaymentGatewayError.InsufficientFunds;
                case "6": errorMessage = "Error Communicating with Bank"; return PaymentGatewayError.PaymentSystemError;
                case "7": errorMessage = "Payment Server detected an error"; return PaymentGatewayError.PaymentSystemError;
                case "8": errorMessage = "Transaction Type Not Supported"; return PaymentGatewayError.UnauthorizedTransaction;
                case "9": errorMessage = "Bank declined transaction (Do not contact Bank)"; return PaymentGatewayError.TransactionDeclined;
                case "A": errorMessage = "Transaction Aborted"; return PaymentGatewayError.TransactionAborted; ;
                case "B": errorMessage = "Transaction Declined - Contact the Bank"; return PaymentGatewayError.TransactionDeclined;
                case "C": errorMessage = "Transaction Cancelled"; return PaymentGatewayError.TransactionCancelled;
                case "D": errorMessage = "Deferred transaction has been received and is awaiting processing"; return PaymentGatewayError.PaymentSystemError;
                case "F": errorMessage = "3-D Secure Authentication failed"; return PaymentGatewayError.ThreeDSecureAuthenticationFailed;
                case "I": errorMessage = "Card Security Code verification failed"; return PaymentGatewayError.InvalidCvv;
                case "L": errorMessage = "Shopping Transaction Locked (Please try the transaction again later)"; return PaymentGatewayError.PaymentSystemError;
                case "N": errorMessage = "Cardholder is not enrolled in Authentication scheme"; return PaymentGatewayError.CardHolderAuthorizationFailed;
                case "P": errorMessage = "Transaction has been received by the Payment Adaptor and is being processed"; return PaymentGatewayError.PaymentSystemError;
                case "R": errorMessage = "Transaction was not processed - Reached limit of retry attempts allowed"; return PaymentGatewayError.ExceededTransactionAttemptLimit;
                case "S": errorMessage = "Duplicate SessionID"; return PaymentGatewayError.DuplicateSessionId;
                case "T": errorMessage = "Address Verification Failed"; return PaymentGatewayError.AddressVerificationFailed;
                case "U": errorMessage = "Card Security Code Failed"; return PaymentGatewayError.InvalidCvv;
                case "V": errorMessage = "Address Verification and Card Security Code Failed"; return PaymentGatewayError.AddressVerificationFailed;
                default: return PaymentGatewayError.Unknown;
            }
        }
    }
}