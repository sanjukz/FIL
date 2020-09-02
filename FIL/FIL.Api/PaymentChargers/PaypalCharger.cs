using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FIL.Api.PaymentChargers
{
    public interface IPaypalCharger<ICharge, IPaymentRedirectResponse> : IPaymentRedirectCharger<ICharge, IPaymentRedirectResponse>
    {
        IPaymentResponse PaypalResponseHandler(IGatewayCharge gatewayCharge);
    }

    public class PaypalCharger : PaymentRedirectCharger<ICharge>, IPaypalCharger<ICharge, IPaymentRedirectResponse>
    {
        private const string Signature = "SIGNATURE", Pwd = "PWD", Acct = "ACCT", Subject = "";

        public PaypalCharger(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        protected override async Task<IPaymentRedirectResponse> PaymentRedirectChargeGenerator(ICharge charge)
        {
            try
            {
                NVPCodec encoder = new NVPCodec();
                encoder["METHOD"] = "SetExpressCheckout";
                encoder["RETURNURL"] = _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Paypal.ReturnUrl);
                encoder["CANCELURL"] = _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Paypal.CancelUrl);
                encoder["PAYMENTREQUEST_0_AMT"] = charge.Amount.ToString();
                encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
                encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = "USD";

                string nvpRequest = encoder.ConvertToNvpString();
                string nvpResponse = HttpNvpRequestHandler(nvpRequest);

                NVPCodec decoder = new NVPCodec();
                decoder.ConvertToNameValueCollection(nvpResponse);

                string acknowledgementStatus = decoder["ACK"].ToLower();
                if (acknowledgementStatus != null && (acknowledgementStatus == "success" || acknowledgementStatus == "successwithwarning"))
                {
                    string expressCheckoutUrl = "https://" + _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Paypal.HostUrl) + "/cgi-bin/webscr?cmd=_express-checkout" + "&token=" + decoder["TOKEN"];
                    return GetPaymentRedirectResponse(expressCheckoutUrl);
                }
                else
                {
                    return GetPaymentRedirectResponse(null, decoder["L_SHORTMESSAGE0"].ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to create redirect charge", ex));
                return GetPaymentRedirectResponse(null, ex.Message);
            }
        }

        public IPaymentResponse PaypalResponseHandler(IGatewayCharge gatewayCharge)
        {
            NameValueCollection nvcResponseQueryString = HttpUtility.ParseQueryString(gatewayCharge.QueryString);
            var token = nvcResponseQueryString["TOKEN"];
            var payerId = nvcResponseQueryString["PAYERID"];

            try
            {
                if (token != null && payerId != null)
                {
                    NVPCodec encoder = new NVPCodec();
                    NVPCodec decoder = new NVPCodec();
                    encoder["METHOD"] = "DoExpressCheckoutPayment";
                    encoder["TOKEN"] = token;
                    encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
                    encoder["PAYERID"] = payerId;
                    encoder["PAYMENTREQUEST_0_AMT"] = gatewayCharge.Amount.ToString();
                    encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = "USD";

                    string nvpRequest = encoder.ConvertToNvpString();
                    string nvpResponse = HttpNvpRequestHandler(nvpRequest);

                    decoder.ConvertToNameValueCollection(nvpResponse);

                    string acknowledgementStatus = decoder["ACK"].ToLower();
                    if (acknowledgementStatus != null && (acknowledgementStatus == "success" || acknowledgementStatus == "successwithwarning"))
                    {
                        string paypalEmail = GetExpressCheckoutDetails(token, payerId);
                        var paymentConfirmationNumber = string.Empty;

                        if (decoder["PAYMENTINFO_0_TRANSACTIONID"] != null)
                        {
                            paymentConfirmationNumber = decoder["PAYMENTINFO_0_TRANSACTIONID"].ToLower();
                        }

                        if (!string.IsNullOrWhiteSpace(paymentConfirmationNumber))
                        {
                            if (!string.IsNullOrWhiteSpace(paypalEmail))
                            {
                                paymentConfirmationNumber = paypalEmail + "~" + paymentConfirmationNumber;
                            }
                        }
                        else
                        {
                            paymentConfirmationNumber = paypalEmail;
                        }

                        return GetPaymentResponse(true, PaymentGatewayError.None);
                    }
                    else
                    {
                        return GetPaymentResponse(false, GetPaymentGatewayErrorCode(decoder["L_ERRORCODE0"].ToString()));
                    }
                }
                else
                {
                    return token != null ? GetPaymentResponse(false, PaymentGatewayError.InvalidToken) : GetPaymentResponse(false, PaymentGatewayError.InvalidPayerId);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to handle response", ex));
                return GetPaymentResponse(false, GetPaymentGatewayErrorCode(ex.Message));
            }
        }

        private string HttpNvpRequestHandler(string nvpRequest)
        {
            const int Timeout = 15000;
            nvpRequest += "&" + SetApiCredentials() + "&BUTTONSOURCE=" + HttpUtility.UrlEncode("");

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Paypal.EndPoint));
            httpWebRequest.Timeout = Timeout;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = nvpRequest.Length;

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(nvpRequest);
            }

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string nvpResponse = string.Empty;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                nvpResponse = streamReader.ReadToEnd();
            }
            return nvpResponse;
        }

        private string SetApiCredentials()
        {
            NVPCodec codec = new NVPCodec();
            if (!string.IsNullOrWhiteSpace(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Paypal.ApiUsername)))
            {
                codec["USER"] = _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Paypal.ApiUsername);
            }

            if (!string.IsNullOrWhiteSpace(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Paypal.ApiPassword)))
            {
                codec[Pwd] = _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Paypal.ApiPassword);
            }

            if (!string.IsNullOrWhiteSpace(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Paypal.ApiSignature)))
            {
                codec[Signature] = _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Paypal.ApiSignature);
            }
            if (!string.IsNullOrWhiteSpace(Subject))
            {
                codec["SUBJECT"] = Subject;
            }

            codec["VERSION"] = "65.0";

            return codec.ConvertToNvpString();
        }

        public string GetExpressCheckoutDetails(string token, string payerId)
        {
            string paypalEmail = string.Empty, shippingAddress = string.Empty, errorMessage = string.Empty;

            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "GetExpressCheckoutDetails";
            encoder["TOKEN"] = token;

            string nvpRequest = encoder.ConvertToNvpString();
            string nvpResponse = HttpNvpRequestHandler(nvpRequest);

            NVPCodec decoder = new NVPCodec();
            decoder.ConvertToNameValueCollection(nvpResponse);

            string acknowledgementStatus = decoder["ACK"].ToLower();
            if (!string.IsNullOrWhiteSpace(acknowledgementStatus) && (acknowledgementStatus == "success" || acknowledgementStatus == "successwithwarning"))
            {
                paypalEmail = decoder["email"];
                //shippingAddress = $"CustomerName: {decoder["PAYMENTREQUEST_0_SHIPTONAME"]} AddressLine: {decoder["PAYMENTREQUEST_0_SHIPTOSTREET"]} AddressLine2: {decoder["PAYMENTREQUEST_0_SHIPTOSTREET2"]} City: {decoder["PAYMENTREQUEST_0_SHIPTOCITY"]} State: {decoder["PAYMENTREQUEST_0_SHIPTOSTATE"]} Zipcode:  {decoder["PAYMENTREQUEST_0_SHIPTOZIP"]}";
            }
            return paypalEmail;
        }

        protected PaymentGatewayError GetPaymentGatewayErrorCode(string errorCode)
        {
            string errorMessage = string.Empty;
            switch (errorCode)
            {
                case "10400": errorMessage = "OrderTotal is missing."; return PaymentGatewayError.InvalidAmount;
                case "10401": errorMessage = "Order total is invalid."; return PaymentGatewayError.InvalidAmount;
                case "10402": errorMessage = "Authorization only is not allowed for merchant."; return PaymentGatewayError.MerchantAuthorizationFailed;
                case "10404": errorMessage = "ReturnURL is missing."; return PaymentGatewayError.InvalidReturnUrl;
                case "10405": errorMessage = "CancelURL is missing."; return PaymentGatewayError.InvalidCancelUrl;
                case "10406": errorMessage = "The PayerID value is invalid."; return PaymentGatewayError.InvalidPayerId;
                case "10407": errorMessage = "Invalid buyer email address (BuyerEmail)."; return PaymentGatewayError.InvalidBuyerEmailAddress;
                case "10408": errorMessage = "Express Checkout token is missing."; return PaymentGatewayError.InvalidToken;
                case "10410": errorMessage = "Invalid token."; return PaymentGatewayError.InvalidToken;
                case "10411": errorMessage = "This Express Checkout session has expired. Token value is no longer valid."; return PaymentGatewayError.SessionExpired;
                case "10416": errorMessage = "You have exceeded the maximum number of payment attempts for this token."; return PaymentGatewayError.ExceededTokenLimit;
                case "10419": errorMessage = "Express Checkout PayerID is missing."; return PaymentGatewayError.InvalidPayerId;
                case "10424": errorMessage = "Shipping address is invalid."; return PaymentGatewayError.InvalidShippingAddress;
                case "10425": errorMessage = "Express Checkout has been disabled for this merchant. Please contact Customer Service."; return PaymentGatewayError.MerchantAuthorizationFailed;
                case "10468": errorMessage = "Duplicate Order Id."; return PaymentGatewayError.DuplicateOrderId;
                case "10471": errorMessage = "ReturnURL is invalid."; return PaymentGatewayError.InvalidReturnUrl;
                case "10472": errorMessage = "CancelURL is invalid."; return PaymentGatewayError.InvalidCancelUrl;
                case "10502": errorMessage = "This transaction cannot be processed. Please use a valid credit card."; return PaymentGatewayError.InvalidCardNumber;
                case "10504": errorMessage = "This transaction cannot be processed. Please enter a valid Credit Card Verification Number."; return PaymentGatewayError.InvalidCvv;
                case "10508": errorMessage = "This transaction cannot be processed. Please enter a valid credit card expiration date."; return PaymentGatewayError.InvalidExpirationDate;
                case "10510": errorMessage = "The credit card type is not supported. Try another card type."; return PaymentGatewayError.CardNotSupported;
                case "10519": errorMessage = "Please enter a credit card."; return PaymentGatewayError.InvalidCardNumber;
                case "10521": errorMessage = "This transaction cannot be processed. Please enter a valid credit card."; return PaymentGatewayError.InvalidCardNumber;
                case "10525": errorMessage = "This transaction cannot be processed. The amount to be charged is zero."; return PaymentGatewayError.InvalidAmount;
                case "10526": errorMessage = "This transaction cannot be processed. The currency is not supported at this time."; return PaymentGatewayError.InvalidCurrency;
                case "10542": errorMessage = "This transaction cannot be processed. Please enter a valid email address."; return PaymentGatewayError.InvalidBuyerEmailAddress;
                case "10562": errorMessage = "This transaction cannot be processed. Please enter a valid credit card expiration year."; return PaymentGatewayError.InvalidExpirationYear;
                case "10563": errorMessage = "This transaction cannot be processed. Please enter a valid credit card expiration month."; return PaymentGatewayError.InvalidExpirationMonth;
                case "10486": errorMessage = "This transaction couldn't be completed. Please redirect your customer to PayPal."; return PaymentGatewayError.UnableToProcessTransaction;
                default: return PaymentGatewayError.Unknown;
            }
        }
    }

    public sealed class NVPCodec : NameValueCollection
    {
        //Returns nvp string of all name/value pairs in the Hashtable
        public string ConvertToNvpString()
        {
            var sb = new StringBuilder();

            bool firstPair = true;
            foreach (string key in AllKeys)
            {
                string name = HttpUtility.UrlEncode(key);
                string value = HttpUtility.UrlEncode(this[key]);
                if (firstPair)
                {
                    sb.Append(string.Format("{0}={1}", name, value));
                    firstPair = false;
                }
                else
                {
                    sb.Append(string.Format("&{0}={1}", name, value));
                }
            }

            return sb.ToString();
        }

        //Converts the nvp string to name-value collection
        public void ConvertToNameValueCollection(string nvpString)
        {
            Clear();
            foreach (string nvp in nvpString.Split(new[] { '&' }))
            {
                string[] tokens = nvp.Split(new[] { '=' });
                if (tokens.Length >= 2)
                {
                    string name = HttpUtility.UrlDecode(tokens[0]);
                    string value = HttpUtility.UrlDecode(tokens[1]);
                    Add(name, value);
                }
            }
        }
    }
}