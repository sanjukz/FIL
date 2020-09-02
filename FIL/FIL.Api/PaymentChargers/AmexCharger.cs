using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FIL.Api.PaymentChargers
{
    public interface IAmexCharger<ICharge, IPaymentRedirectResponse> : IPaymentRedirectCharger<ICharge, IPaymentRedirectResponse>
    {
        IPaymentResponse AmexResponseHandler(IGatewayCharge gatewayCharge);
    }

    public class AmexCharger : PaymentRedirectCharger<ICharge>, IAmexCharger<ICharge, IPaymentRedirectResponse>
    {
        public AmexCharger(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        protected override async Task<IPaymentRedirectResponse> PaymentRedirectChargeGenerator(ICharge charge)
        {
            try
            {
                VpcRequest vpcRequest = new VpcRequest(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.VpcPaymentServerUrl), _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.SecureSecret));
                vpcRequest.AddVpcRequestFields("vpc_Version", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.VpcVersion));
                vpcRequest.AddVpcRequestFields("vpc_Command", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.VpcCommand));
                vpcRequest.AddVpcRequestFields("vpc_AccessCode", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.AccessKey));
                vpcRequest.AddVpcRequestFields("vpc_Merchant", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.MerchantId));
                vpcRequest.AddVpcRequestFields("vpc_ReturnURL", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Paypal.ReturnUrl));
                vpcRequest.AddVpcRequestFields("vpc_MerchTxnRef", charge.TransactionId.ToString());
                vpcRequest.AddVpcRequestFields("vpc_OrderInfo", $"Transaction charge for {charge.TransactionId.ToString()}");
                vpcRequest.AddVpcRequestFields("vpc_Amount", (Convert.ToDouble(charge.Amount) * 100).ToString());
                vpcRequest.AddVpcRequestFields("vpc_Locale", "en");

                string chargeUrl = vpcRequest.CreateAmexChargeRedirectUrl();
                return GetPaymentRedirectResponse(chargeUrl, "");
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to create redirect charge", ex));
                return GetPaymentRedirectResponse(null, ex.Message);
            }
        }

        public IPaymentResponse AmexResponseHandler(IGatewayCharge gatewayCharge)
        {
            NameValueCollection nvcResponseQueryString = HttpUtility.ParseQueryString(gatewayCharge.QueryString);

            VpcRequest vpcRequest = new VpcRequest(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.VpcPaymentCaptureServerUrl), _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.SecureSecret));
            vpcRequest.AddVpcResponseFields(nvcResponseQueryString);

            string vpcTxnResponseCode = vpcRequest.GetResultField("vpc_TxnResponseCode", "Unknown");
            string vpc3DSstatus = vpcRequest.GetResultField("vpc_3DSstatus", "Unknown");
            string vpcTransactionNo = vpcRequest.GetResultField("vpc_TransactionNo", "Unknown");
            string vpcMerchTxnRef = vpcRequest.GetResultField("vpc_MerchTxnRef", "Unknown");
            string vpcAmount = vpcRequest.GetResultField("vpc_Amount", "Unknown");

            if (vpcTxnResponseCode.Equals("0") && vpc3DSstatus.Equals("Y"))
            {
                try
                {
                    VpcRequest vpcCaptureRequest = new VpcRequest(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.VpcPaymentCaptureServerUrl), _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.SecureSecret));

                    vpcCaptureRequest.AddVpcRequestFields("vpc_Version", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.VpcVersion));
                    vpcCaptureRequest.AddVpcRequestFields("vpc_Command", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.VpcCommand));
                    vpcCaptureRequest.AddVpcRequestFields("vpc_AccessCode", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.AccessKey));
                    vpcCaptureRequest.AddVpcRequestFields("vpc_Merchant", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.MerchantId));
                    vpcCaptureRequest.AddVpcRequestFields("vpc_TransNo", vpcTransactionNo);
                    vpcCaptureRequest.AddVpcRequestFields("vpc_MerchTxnRef", vpcMerchTxnRef);
                    vpcCaptureRequest.AddVpcRequestFields("vpc_Amount", vpcAmount);
                    vpcCaptureRequest.AddVpcRequestFields("vpc_User", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.UserName));
                    vpcCaptureRequest.AddVpcRequestFields("vpc_Password", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Amex.Password));

                    vpcCaptureRequest.SendVpcCaptureRequest();

                    string vpcCaptureTxnResponseCode = vpcRequest.GetResultField("vpc_TxnResponseCode", "Unknown");
                    string paymentConfirmationNumber = vpcRequest.GetResultField("vpc_TransactionNo", "Unknown");
                    string vpcCaptureMerchTxnRef = vpcRequest.GetResultField("vpc_MerchTxnRef", "Unknown");

                    if (vpcCaptureTxnResponseCode.Equals("0"))
                    {
                        return GetPaymentResponse(true, PaymentGatewayError.None);
                    }
                    else
                    {
                        return GetPaymentResponse(false, GetPaymentGatewayErrorCode(vpcCaptureTxnResponseCode));
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogCategory.Error, new Exception("Failed to process transaction", ex));
                    return GetPaymentResponse(false, GetPaymentGatewayErrorCode(ex.Message));
                }
            }
            else
            {
                return GetPaymentResponse(false, GetPaymentGatewayErrorCode(vpcTxnResponseCode));
            }
        }

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

    public class VpcRequest
    {
        private Uri _address;
        private SortedList<String, String> vpcRequestFields = new SortedList<String, String>(new VpcStringComparer());
        private SortedList<String, String> vpcResponseFields = new SortedList<String, String>(new VpcStringComparer());
        private string proxyHost = string.Empty, proxyUser = string.Empty, proxyPassword = string.Empty, proxyDomain = string.Empty, _secureSecret = string.Empty, vpcRawResponse = string.Empty;

        public VpcRequest(string address, string secureSecret)
        {
            _address = new Uri(address);
            _secureSecret = secureSecret;
        }

        public void AddVpcRequestFields(String key, String value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                vpcRequestFields.Add(key, value);
            }
        }

        public String CreateAmexChargeRedirectUrl()
        {
            return $"{_address}?{GetVpcRequestRaw()}&vpc_SecureHash={GenerateSHA256Signature(true)}&vpc_SecureHashType=SHA256";
        }

        private string GetVpcRequestRaw()
        {
            StringBuilder vpcRequestRaw = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in vpcRequestFields)
            {
                if (!string.IsNullOrWhiteSpace(keyValuePair.Value))
                {
                    vpcRequestRaw.Append($"{keyValuePair.Key}={HttpUtility.UrlEncode(keyValuePair.Value)}&");
                }
            }
            return vpcRequestRaw.ToString().TrimEnd('&');
        }

        private string GenerateSHA256Signature(bool useRequestFields)
        {
            StringBuilder vpcFields = new StringBuilder();
            SortedList<String, String> inputFields = (useRequestFields ? vpcRequestFields : vpcResponseFields);

            foreach (KeyValuePair<string, string> keyValuePair in inputFields)
            {
                if (keyValuePair.Key.StartsWith("vpc_") || keyValuePair.Key.StartsWith("user_"))
                    vpcFields.Append($"{keyValuePair.Key}={keyValuePair.Value}&");
            }

            byte[] convertedHash = new byte[_secureSecret.Length / 2];
            for (int i = 0; i < _secureSecret.Length / 2; i++)
            {
                convertedHash[i] = (byte)Int32.Parse(_secureSecret.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            string secureHashValue = string.Empty;
            using (HMACSHA256 hasher = new HMACSHA256(convertedHash))
            {
                byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(vpcFields.ToString().TrimEnd('&')));
                foreach (byte b in hashValue)
                {
                    secureHashValue += b.ToString("X2");
                }
            }
            return secureHashValue;
        }

        public void AddVpcResponseFields(NameValueCollection nvcVpcResponseFields)
        {
            foreach (string vpcResponseField in nvcVpcResponseFields)
            {
                if (!vpcResponseField.Equals("vpc_SecureHash") && !vpcResponseField.Equals("vpc_SecureHashType"))
                {
                    vpcResponseFields.Add(vpcResponseField, nvcVpcResponseFields[vpcResponseField]);
                }
            }

            if (!nvcVpcResponseFields["vpc_TxnResponseCode"].Equals("0") && !string.IsNullOrWhiteSpace(nvcVpcResponseFields["vpc_Message"]))
            {
                if (!string.IsNullOrWhiteSpace(nvcVpcResponseFields["vpc_SecureHash"]))
                {
                    if (!GenerateSHA256Signature(false).Equals(nvcVpcResponseFields["vpc_SecureHash"]))
                    {
                        throw new Exception("Secure Hash does not match");
                    }
                    return;
                }
                return;
            }

            if (string.IsNullOrWhiteSpace(vpcResponseFields["vpc_SecureHash"]))
            {
                throw new Exception("No Secure Hash included in response");
            }
        }

        public string GetResultField(string key, string defaultValue)
        {
            string value;
            return vpcResponseFields.TryGetValue(key, out value) == true ? value : defaultValue;
        }

        public void SendVpcCaptureRequest()
        {
            if (!string.IsNullOrWhiteSpace(proxyHost))
            {
                WebProxy proxy = new WebProxy(proxyHost, true);
                if (!string.IsNullOrWhiteSpace(proxyUser))
                {
                    if (string.IsNullOrWhiteSpace(proxyPassword))
                    {
                        proxyPassword = "";
                    }

                    if (string.IsNullOrWhiteSpace(proxyDomain))
                    {
                        proxy.Credentials = new NetworkCredential(proxyUser, proxyPassword);
                    }
                    else
                    {
                        proxy.Credentials = new NetworkCredential(proxyUser, proxyPassword, proxyDomain);
                    }
                }
                WebRequest.DefaultWebProxy = proxy;
            }

            HttpWebRequest request = WebRequest.Create(_address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrWhiteSpace(_secureSecret))
            {
                vpcRequestFields.Add("vpc_SecureHash", GenerateSHA256Signature(true));
                vpcRequestFields.Add("vpc_SecureHashType", "SHA256");
            }

            byte[] byteData = UTF8Encoding.UTF8.GetBytes(GetVpcRequestRaw());
            request.ContentLength = byteData.Length;

            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                vpcRawResponse = reader.ReadToEnd();
                string[] responses = vpcRawResponse.Split('&');
                foreach (string responseField in responses)
                {
                    string[] field = responseField.Split('=');
                    vpcResponseFields.Add(field[0], HttpUtility.UrlDecode(field[1]));
                }
            }

            if (!string.IsNullOrWhiteSpace(_secureSecret))
            {
                try
                {
                    string secureHash = vpcResponseFields["vpc_SecureHash"];
                    string secureHashType = vpcResponseFields["vpc_SecureHashType"];
                    vpcResponseFields.Remove("vpc_SecureHash");
                    vpcResponseFields.Remove("vpc_SecureHashType");

                    if (string.IsNullOrWhiteSpace(secureHash))
                    {
                        throw new Exception("Secure Hash not returned from VPC");
                    }
                    else if (!secureHash.Equals(GenerateSHA256Signature(false)))
                    {
                        throw new Exception("Secure Hash returned from VPC does not match");
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private class VpcStringComparer : IComparer<String>
        {
            public int Compare(String a, String b)
            {
                if (a == b) return 0;
                if (a == null) return -1;
                if (b == null) return 1;

                string sa = a as string;
                string sb = b as string;

                System.Globalization.CompareInfo myComparer = System.Globalization.CompareInfo.GetCompareInfo("en-US");
                if (sa != null && sb != null)
                {
                    return myComparer.Compare(sa, sb, System.Globalization.CompareOptions.Ordinal);
                }
                throw new ArgumentException("a and b should be strings.");
            }
        }
    }
}