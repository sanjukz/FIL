using CCA.Util;
using FIL.Api.Providers.Transaction;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FIL.Api.PaymentChargers
{
    public interface ICcavenueCharger<ICcavenueCharge, IPaymentHtmlPostResponse> : IPaymentHtmlPostCharger<ICcavenueCharge, IPaymentHtmlPostResponse>
    {
        IPaymentResponse CcavenueResponseHandler(IGatewayCharge gatewayCharge);
    }

    public class CcavenueCharger : PaymentHtmlPostCharger<ICcavenueCharge>, ICcavenueCharger<ICcavenueCharge, IPaymentHtmlPostResponse>
    {
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly INetBankingBankDetailRepository _netBankingBankDetailRepository;
        private readonly ICashCardDetailRepository _cashCardDetailRepository;
        private readonly ITransactionStatusUpdater _transactionStatusUpdater;

        public CcavenueCharger(ILogger logger, ISettings settings, ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            INetBankingBankDetailRepository netBankingBankDetailRepository,
            ICashCardDetailRepository cashCardDetailRepository, ITransactionStatusUpdater transactionStatusUpdater)
          : base(logger, settings)
        {
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _netBankingBankDetailRepository = netBankingBankDetailRepository;
            _cashCardDetailRepository = cashCardDetailRepository;
            _transactionStatusUpdater = transactionStatusUpdater;
        }

        protected override async Task<IPaymentHtmlPostResponse> PaymentHtmlPostChargeGenerator(ICcavenueCharge ccavenueCharge)
        {
            try
            {
                Dictionary<string, string> formFields = new Dictionary<string, string>();
                formFields.Add("encRequest", GetCcavenueEncryptedChargeRequest(ccavenueCharge, _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Ccavenue.MerchantId), _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Ccavenue.WorkingKey),
                  ccavenueCharge.ChannelId == Channels.Feel ? _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Ccavenue.FeelReturnUrl) : _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Ccavenue.ReturnUrl)));
                formFields.Add("access_code", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Ccavenue.AccessKey));

                IHtmlPostRequest htmlPostRequest = new HtmlPostRequest();
                htmlPostRequest.Method = "POST";
                htmlPostRequest.Action = $"{_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Ccavenue.PostUrl)}";
                htmlPostRequest.FormFields = formFields;
                string returnUrl = ccavenueCharge.ChannelId == Channels.Feel ? _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Ccavenue.FeelReturnUrl) : _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Ccavenue.ReturnUrl);

                _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                {
                    TransactionId = Convert.ToInt64(ccavenueCharge.TransactionId),
                    PaymentOptionId = ccavenueCharge.PaymentOption == PaymentOptions.CashCard ? PaymentOptions.CashCard : PaymentOptions.NetBanking,
                    PaymentGatewayId = PaymentGateway.CCAvenue,
                    UserCardDetailId = ccavenueCharge.UserCardDetailId,
                    RequestType = "Charge Posting",
                    Amount = ccavenueCharge.Amount.ToString(),
                    PayConfNumber = "",
                    PaymentDetail = "{\"Request\":" + Newtonsoft.Json.JsonConvert.SerializeObject(htmlPostRequest) + ",\"ReturnUrl\":\"" + returnUrl + "\"}",
                });
                return GetPaymentHtmlPostResponse(htmlPostRequest);
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to generate html post request", ex));
                return GetPaymentHtmlPostResponse(null, ex.Message);
            }
        }

        public string GetCcavenueEncryptedChargeRequest(ICcavenueCharge ccavenueCharge, string merchantId, string workingKey, string returnUrl)
        {
            StringBuilder ccavenueRequest = new StringBuilder();
            CCACrypto ccaCrypto = new CCACrypto();
            ccavenueRequest.Append($"merchant_id={merchantId}&order_id={ccavenueCharge.TransactionId}&amount={ccavenueCharge.Amount}&currency={ccavenueCharge.Currency.ToUpper()}&redirect_url={returnUrl}&cancel_url={returnUrl}&");

            ccavenueRequest.Append($"billing_name={ccavenueCharge.BillingAddress.FirstName} {ccavenueCharge.BillingAddress.LastName}&billing_address={ccavenueCharge.BillingAddress.AddressLine1}&billing_city={ccavenueCharge.BillingAddress.City}&billing_state={ccavenueCharge.BillingAddress.State}&billing_zip={ccavenueCharge.BillingAddress.Zipcode}&billing_country={ccavenueCharge.BillingAddress.Country}&billing_tel={ccavenueCharge.BillingAddress.PhoneCode}{ccavenueCharge.BillingAddress.PhoneNumber}&billing_email={ccavenueCharge.BillingAddress.Email}&");

            ccavenueRequest.Append($"delivery_name={ccavenueCharge.BillingAddress.FirstName} {ccavenueCharge.BillingAddress.LastName}&delivery_address={ccavenueCharge.BillingAddress.AddressLine1}&delivery_city={ccavenueCharge.BillingAddress.City}&delivery_state={ccavenueCharge.BillingAddress.State}&delivery_zip={ccavenueCharge.BillingAddress.Zipcode}&delivery_country={ccavenueCharge.BillingAddress.Country}&delivery_tel={ccavenueCharge.BillingAddress.PhoneCode}{ccavenueCharge.BillingAddress.PhoneNumber}&");

            var paymentOption = GetCcavenuePaymentOptions(ccavenueCharge.PaymentOption);
            ccavenueRequest.Append($"payment_option={paymentOption.Item1}&card_type={paymentOption.Item2}&");
            if (ccavenueCharge.PaymentOption == PaymentOptions.NetBanking)
            {
                var bankName = _netBankingBankDetailRepository.GetByAltId(ccavenueCharge.BankAltId).BankName;
                ccavenueRequest.Append($"card_name={bankName}&");
            }
            else if (ccavenueCharge.PaymentOption == PaymentOptions.CashCard)
            {
                var cardName = _cashCardDetailRepository.GetByAltId(ccavenueCharge.CardAltId).CardName;
                ccavenueRequest.Append($"card_name={cardName}&");
            }
            else
            {
                ccavenueRequest.Append($"card_number={ccavenueCharge.PaymentCard.CardNumber}&expiry_month={ccavenueCharge.PaymentCard.ExpiryMonth}&expiry_year={ccavenueCharge.PaymentCard.ExpiryYear}&cvv_number={ccavenueCharge.PaymentCard.Cvv}&");
            }
            ccavenueRequest.Append($"data_accept=N&");

            return ccaCrypto.Encrypt(ccavenueRequest.ToString(), workingKey);
        }

        public IPaymentResponse CcavenueResponseHandler(IGatewayCharge gatewayResponse)
        {
            //NameValueCollection nvcResponseQueryString = HttpUtility.ParseQueryString(gatewayResponse.QueryString);
            try
            {
                CCACrypto ccaCrypto = new CCACrypto();
                NameValueCollection nvcEncResponse = HttpUtility.ParseQueryString(gatewayResponse.QueryString);
                var encResponseValue = nvcEncResponse["encResp"];
                string encResponse = ccaCrypto.Decrypt(encResponseValue, _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Ccavenue.WorkingKey));

                NameValueCollection nvcResponseQueryString = new NameValueCollection();
                string[] segments = encResponse.Split('&');
                foreach (string seg in segments)
                {
                    string[] parts = seg.Split('=');
                    if (parts.Length > 0)
                    {
                        string Key = parts[0].Trim();
                        string Value = parts[1].Trim();
                        nvcResponseQueryString.Add(Key, Value);
                    }
                }

                var orderId = nvcResponseQueryString["order_id"];
                var paymentConfirmationNumber = nvcResponseQueryString["tracking_id"];
                var orderStatus = nvcResponseQueryString["order_status"];
                var statusMessage = nvcResponseQueryString["status_message"];
                var amount = nvcResponseQueryString["amount"];
                var paymentMode = nvcResponseQueryString["payment_mode"];

                bool payStatus = orderStatus.ToUpper().Equals("SUCCESS") && !paymentConfirmationNumber.ToUpper().Equals("FAIL") && !string.IsNullOrWhiteSpace(paymentConfirmationNumber);

                if (payStatus)
                {
                    _transactionStatusUpdater.UpdateTranscationStatus(Convert.ToInt64(orderId));
                }

                _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                {
                    TransactionId = Convert.ToInt64(orderId),
                    PaymentOptionId = paymentMode == "Net Banking" ? PaymentOptions.NetBanking : PaymentOptions.CashCard,
                    PaymentGatewayId = PaymentGateway.CCAvenue,
                    RequestType = "Charge Recieved",
                    Amount = amount != null ? amount.ToString() : "",
                    PayConfNumber = paymentConfirmationNumber,
                    PaymentDetail = "{\"Response\":" + Newtonsoft.Json.JsonConvert.SerializeObject(nvcResponseQueryString.AllKeys.ToDictionary(k => k, k => nvcResponseQueryString[k])) + ",\"encRepsonse\":" + encResponseValue + "}",
                });
                return GetPaymentResponse(payStatus, payStatus ? PaymentGatewayError.None : GetPaymentGatewayErrorCode(string.IsNullOrWhiteSpace(statusMessage) ? "Transaction declined" : statusMessage));
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to process transaction", ex));
                return GetPaymentResponse(false, GetPaymentGatewayErrorCode(ex.Message));
            }
        }

        private (string, string) GetCcavenuePaymentOptions(PaymentOptions paymentOption)
        {
            string paymentOtpion = string.Empty;
            string cardType = string.Empty;
            switch (paymentOption)
            {
                case PaymentOptions.CreditCard:
                    paymentOtpion = "OPTCRDC";
                    cardType = "CRDC";
                    break;

                case PaymentOptions.DebitCard:
                    paymentOtpion = "OPTDBCRD";
                    cardType = "DBCRD";
                    break;

                case PaymentOptions.NetBanking:
                    paymentOtpion = "OPTNBK";
                    cardType = "NBK";
                    break;

                case PaymentOptions.CashCard:
                    paymentOtpion = "OPTWLT";
                    cardType = "WLT";
                    break;
            }
            return (paymentOtpion, cardType);
        }

        protected PaymentGatewayError GetPaymentGatewayErrorCode(string errorMessage)
        {
            string errorDescription = string.Empty;
            switch (errorMessage)
            {
                case string a when a.Contains("Transaction cannot be authorized"): return PaymentGatewayError.UnauthorizedTransaction;
                case string a when a.Contains("Authentication failed"): return PaymentGatewayError.MerchantAuthorizationFailed;
                case string a when a.Contains("a non-3D card"): return PaymentGatewayError.ThreeDSecureAuthenticationFailed;
                case "Cancelled by User": return PaymentGatewayError.TransactionCancelled;
                case "Do Not Honor": return PaymentGatewayError.TransactionDeclined;
                case string a when a.Contains("not 3D enrolled"): return PaymentGatewayError.ThreeDSecureAuthenticationFailed;
                case string a when a.Contains("sufficient fund"): return PaymentGatewayError.InsufficientFunds;
                case "Payment is UnSuccessful": return PaymentGatewayError.TransactionCancelled;
                case "Rejected By Acquirer": return PaymentGatewayError.TransactionDeclined;
                case "Sorry! Your transaction has been cancelled": return PaymentGatewayError.TransactionCancelled;
                case string a when a.Contains("Transaction aborted"): return PaymentGatewayError.TransactionAborted;
                case string a when a.Contains("Transaction declined"): return PaymentGatewayError.TransactionDeclined;
                case string a when a.Contains("Transaction failed"): return PaymentGatewayError.TransactionDeclined;
                case "Transaction was Cancelled": return PaymentGatewayError.TransactionCancelled;
                case "Transaction status not received from bank.": return PaymentGatewayError.PaymentSystemError;
                case "unable to authorize": return PaymentGatewayError.CardHolderAuthorizationFailed;
                case "Unauthorized usage": return PaymentGatewayError.CardHolderAuthorizationFailed;
                case string a when a.Contains("You are not authorized to do this transaction. "): return PaymentGatewayError.UnauthorizedTransaction;
                case string a when a.Contains("21001"):
                case string b when b.Contains("31001"):
                    errorDescription = "order_id:Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidOrderId;
                case string a when a.Contains("21002"):
                case string b when b.Contains("31002"):
                    errorDescription = "currency: Required paramter missing / Invalid Parameter"; return PaymentGatewayError.InvalidCurrency;
                case string a when a.Contains("21003"):
                case string b when b.Contains("31003"):
                    errorDescription = "amount:Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidAmount;
                case string a when a.Contains("21004"):
                case string b when b.Contains("31004"):
                    errorDescription = "billing_name: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidBillingName;
                case string a when a.Contains("21005"):
                case string b when b.Contains("31005"):
                    errorDescription = "billing_address: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidBillingAddress;
                case string a when a.Contains("21006"):
                case string b when b.Contains("31006"):
                    errorDescription = "billing_city: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidBillingCity;
                case string a when a.Contains("21007"):
                case string b when b.Contains("31007"):
                    errorDescription = "billing_state: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidBillingState;
                case string a when a.Contains("21008"):
                case string b when b.Contains("31008"):
                    errorDescription = "billing_zip: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidBillingZipcode;
                case string a when a.Contains("21009"):
                case string b when b.Contains("31009"):
                    errorDescription = "billing_country: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidBillingCountry;
                case string a when a.Contains("21010"):
                case string b when b.Contains("31010"):
                    errorDescription = "billing_tel: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidBillingPhoneNumber;
                case string a when a.Contains("21011"):
                case string b when b.Contains("31011"):
                    errorDescription = "billing_email: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidBillingEmail;
                case string a when a.Contains("21012"):
                case string b when b.Contains("31012"):
                    errorDescription = "delivery_name: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidDeliveryName;
                case string a when a.Contains("21013"):
                case string b when b.Contains("31013"):
                    errorDescription = "delivery_address: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidDeliveryAddress;
                case string a when a.Contains("21014"):
                case string b when b.Contains("31014"):
                    errorDescription = "delivery_city: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidDeliveryCity;
                case string a when a.Contains("21015"):
                case string b when b.Contains("31015"):
                    errorDescription = "delivery_state: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidDeliveryState;
                case string a when a.Contains("21016"):
                case string b when b.Contains("31016"):
                    errorDescription = "delivery_zip: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidDeliveryZipcode;
                case string a when a.Contains("21017"):
                case string b when b.Contains("31017"):
                    errorDescription = "delivery_country: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidDeliveryCountry;
                case string a when a.Contains("21018"):
                case string b when b.Contains("31018"):
                    errorDescription = "delivery_tel: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidDeliveryPhoneNumber;
                case string a when a.Contains("21020"):
                case string b when b.Contains("31020"):
                    errorDescription = "card_name: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidNamOnCard;
                case string a when a.Contains("21021"):
                case string b when b.Contains("31021"):
                    errorDescription = "card_type: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidCardType;
                case string a when a.Contains("21022"):
                case string b when b.Contains("31022"):
                    errorDescription = "payment_option: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidPaymentOption;
                case string a when a.Contains("21023"):
                case string b when b.Contains("31023"):
                    errorDescription = "card_number: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidCardNumber;
                case string a when a.Contains("21024"):
                case string b when b.Contains("31024"):
                    errorDescription = "expiry_month: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidExpirationMonth;
                case string a when a.Contains("21025"):
                case string b when b.Contains("31025"):
                    errorDescription = "expiry_year: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidExpirationYear;
                case string a when a.Contains("21026"):
                case string b when b.Contains("31026"):
                    errorDescription = "cvv_number: Required parameter missing / Invalid Parameter"; return PaymentGatewayError.InvalidCvv;
                default: return PaymentGatewayError.Unknown;
            }
        }
    }
}