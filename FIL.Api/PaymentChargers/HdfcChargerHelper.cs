using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using System.IO;
using System.Net;
using System.Text;

namespace FIL.Api.PaymentChargers
{
    public class HdfcChargerHelper
    {
        public static string GetHdfcChargeRequest(IHdfcCharge hdfcCharge, string tranportalId, string tranportalPassword)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<ID>{tranportalId}</ID>");
            sb.Append($"<password>{tranportalPassword}</password>");
            sb.Append($"<card>{hdfcCharge.PaymentCard.CardNumber}</card>");
            sb.Append($"<cvv2>{hdfcCharge.PaymentCard.Cvv}</cvv2>");
            sb.Append($"<expyear>{hdfcCharge.PaymentCard.ExpiryYear}</expyear>");
            sb.Append($"<expmonth>{hdfcCharge.PaymentCard.ExpiryMonth}</expmonth>");
            sb.Append($"<action>1</action>");
            sb.Append($"<amt>{hdfcCharge.Amount.ToString("0.00")}</amt>");
            sb.Append($"<currencycode>356</currencycode>");
            sb.Append($"<member>{hdfcCharge.PaymentCard.NameOnCard}</member>");
            sb.Append($"<trackid>{hdfcCharge.TransactionId}</trackid>");
            return sb.ToString();
        }

        public static string HttpWebRequestHandler(string requestUrl, string request)
        {
            string response = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = request.Length;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            streamWriter.Write(request);
            streamWriter.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = sr.ReadToEnd();
            }
            return response;
        }

        public static string GetResultField(string s, string tag)
        {
            int startIndex = s.IndexOf($"<{tag}>") + tag.Length + 2;
            int endIndex = s.IndexOf($"</{tag}>", startIndex);
            return endIndex > 0 ? s.Substring(startIndex, endIndex - startIndex) : "";
        }

        public static PaymentGatewayError GetPaymentGatewayErrorCode(string errorMessage)
        {
            string errorDescription = string.Empty;
            switch (errorMessage)
            {
                case "Transaction declined": return PaymentGatewayError.TransactionDeclined;
                case string a when a.Contains("CM00002"): errorDescription = "External message system error."; return PaymentGatewayError.PaymentSystemError;
                case string a when a.Contains("FSS0001"): errorDescription = "Authentication Not Available"; return PaymentGatewayError.MerchantAuthorizationFailed;
                case string a when a.Contains("GV00004"): errorDescription = "PARes status not successful"; return PaymentGatewayError.ThreeDSecureAuthenticationFailed;
                case string a when a.Contains("GV00007"):
                case string b when b.Contains("GV00008"):
                    errorDescription = "Signature validation failed / 3D secure not matching"; return PaymentGatewayError.ThreeDSecureAuthenticationFailed;
                case string a when a.Contains("GV00013"):
                case string b when b.Contains("GV00104"):
                    errorDescription = "Invalid Payment ID"; return PaymentGatewayError.InvalidOrderId;
                case string a when a.Contains("GV00102"):
                case string b when b.Contains("GW00152"):
                    errorDescription = "Invalid Amount"; return PaymentGatewayError.InvalidAmount;
                case string a when a.Contains("GV00103"): errorDescription = "Invalid Brand"; return PaymentGatewayError.CardNotSupported;
                case string a when a.Contains("GW00161"): errorDescription = "Invalid Card/Member Name data."; return PaymentGatewayError.InvalidNamOnCard;
                case string a when a.Contains("GW00166"): errorDescription = "Invalid Card Number data."; return PaymentGatewayError.InvalidCardNumber;
                case string a when a.Contains("GW00456"): errorDescription = "Invalid TranPortal ID."; return PaymentGatewayError.MerchantAuthorizationFailed;
                case string a when a.Contains("GW00458"): errorDescription = "Invalid Transaction Attempt."; return PaymentGatewayError.ExceededTransactionAttemptLimit;
                case string a when a.Contains("GW00856"): errorDescription = "Invalid Card Verification Code."; return PaymentGatewayError.InvalidCvv;
                case string a when a.Contains("CM90000"): errorDescription = "Database error."; return PaymentGatewayError.PaymentSystemError;
                case string a when a.Contains("CM90004"): errorDescription = "Duplicate found error."; return PaymentGatewayError.DuplicateOrderId;
                case string a when a.Contains("CM90003"): errorDescription = "No Records Found."; return PaymentGatewayError.PaymentSystemError;
                default: return PaymentGatewayError.Unknown;
            }
        }
    }
}