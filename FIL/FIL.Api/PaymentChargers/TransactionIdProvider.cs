using CCA.Util;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace FIL.Api.PaymentChargers
{
    public interface ITransactionIdProvider
    {
        TransactionProvider Get(string queryString);
    }

    public class TransactionIdProvider : ITransactionIdProvider
    {
        private readonly ISettings _settings;
        private readonly FIL.Logging.ILogger _logger;

        public TransactionIdProvider(ISettings settings, FIL.Logging.ILogger logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public TransactionProvider Get(string queryString)
        {
            TransactionProvider transactionProvider = new TransactionProvider();
            try
            {
                var nvcResponseQueryString = HttpUtility.ParseQueryString(queryString);
                // Custom redirects
                if (nvcResponseQueryString.AllKeys.Contains("gateway"))
                {
                    var isValid =
                        Enum.TryParse<PaymentGateway>(nvcResponseQueryString["gateway"], out PaymentGateway gateway);
                    if (isValid)
                    {
                        if (gateway == PaymentGateway.Stripe)
                        {
                            var orderId = nvcResponseQueryString["orderId"];
                            transactionProvider.TransactionId =
                                !string.IsNullOrWhiteSpace(orderId) ? Convert.ToInt64(orderId) : 0;
                            transactionProvider.PaymentGateway = PaymentGateway.Stripe;
                            transactionProvider.Token = nvcResponseQueryString["source"];
                            return transactionProvider;
                        }
                    }
                }

                // NAB Transact
                if (nvcResponseQueryString.AllKeys.Contains("refid"))
                {
                    string orderId = nvcResponseQueryString["refid"];
                    transactionProvider.TransactionId = !string.IsNullOrWhiteSpace(orderId) ? Convert.ToInt64(orderId) : 0;
                    transactionProvider.PaymentGateway = PaymentGateway.NabTransact;
                    return transactionProvider;
                }

                // CCAvenue
                if (nvcResponseQueryString.AllKeys.Contains("encResp"))
                {
                    CCACrypto ccaCrypto = new CCACrypto();
                    var encResponseValue = nvcResponseQueryString["encResp"];
                    string encResponse = ccaCrypto.Decrypt(encResponseValue, _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Ccavenue.WorkingKey));

                    NameValueCollection nvcQueryString = new NameValueCollection();
                    string[] segments = encResponse.Split('&');
                    foreach (string seg in segments)
                    {
                        string[] parts = seg.Split('=');
                        if (parts.Length > 0)
                        {
                            string Key = parts[0].Trim();
                            string Value = parts[1].Trim();
                            nvcQueryString.Add(Key, Value);
                        }
                    }
                    string orderId = nvcQueryString["order_id"];
                    transactionProvider.TransactionId = !string.IsNullOrWhiteSpace(orderId) ? Convert.ToInt64(orderId) : 0;
                    transactionProvider.PaymentGateway = PaymentGateway.CCAvenue;
                    return transactionProvider;
                }

                // HDFC
                if (nvcResponseQueryString.AllKeys.Contains("PaRes"))
                {
                    var paymentId = nvcResponseQueryString["MD"];
                    var paymentAuthenticationResponse = nvcResponseQueryString["PaRes"];
                    var response = HdfcChargerHelper.HttpWebRequestHandler(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Hdfc.PayerAuthenticationUrl), GetHdfcEnrolledRequest(paymentId, paymentAuthenticationResponse));
                    var orderId = HdfcChargerHelper.GetResultField(response, "trackid");
                    return new TransactionProvider
                    {
                        TransactionId = !string.IsNullOrWhiteSpace(orderId) ? Convert.ToInt64(orderId) : 0,
                        Response = response,
                        PaymentGateway = PaymentGateway.HDFC
                    };
                }

                if (transactionProvider.TransactionId == 0)
                {
                    _logger.Log(Logging.Enums.LogCategory.Warn, "Payment method not found.", new Dictionary<string, object>
                    {
                        ["QueryString"] = queryString
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }

            return new TransactionProvider();
        }

        private string GetHdfcEnrolledRequest(string paymentId, string paymentAuthenticationResponse)
        {
            StringBuilder sb = new StringBuilder();
            paymentAuthenticationResponse = paymentAuthenticationResponse.Replace(' ', '+');
            sb.Append($"<ID>{_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Hdfc.TranportalId)}</ID>");
            sb.Append($"<password>{_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Hdfc.TranportalPassword)}</password>");
            sb.Append($"<paymentid>{paymentId}</paymentid>");
            sb.Append($"<PARes>{paymentAuthenticationResponse}</PARes>");
            return sb.ToString();
        }
    }
}