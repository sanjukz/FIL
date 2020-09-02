using FIL.Api.Providers.Transaction;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using FIL.Logging.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FIL.Api.PaymentChargers
{
    public interface IHdfcEnrolledCharger<IHdfcEnrolledCharge, IPaymentHtmlPostResponse> : IPaymentHtmlPostCharger<IHdfcEnrolledCharge, IPaymentHtmlPostResponse>
    {
        IPaymentResponse HdfcResponseHandler(IGatewayCharge gatewayCharge);
    }

    public class HdfcEnrolledCharger : PaymentHtmlPostCharger<IHdfcEnrolledCharge>, IHdfcEnrolledCharger<IHdfcEnrolledCharge, IPaymentHtmlPostResponse>
    {
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly ITransactionStatusUpdater _transactionStatusUpdater;

        public HdfcEnrolledCharger(ILogger logger, ISettings settings, ITransactionPaymentDetailRepository transactionPaymentDetailRepository, ITransactionStatusUpdater transactionStatusUpdater)
       : base(logger, settings)
        {
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _transactionStatusUpdater = transactionStatusUpdater;
        }

        protected override async Task<IPaymentHtmlPostResponse> PaymentHtmlPostChargeGenerator(IHdfcEnrolledCharge hdfcEnrolledCharge)
        {
            try
            {
                Dictionary<string, string> formFields = new Dictionary<string, string>();
                formFields.Add("TermUrl", _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Hdfc.ReturnUrl));
                formFields.Add("PaReq", hdfcEnrolledCharge.PaymentAuthenticationRequest);
                formFields.Add("MD", hdfcEnrolledCharge.PaymentId);
                formFields.Add("TransId", hdfcEnrolledCharge.TransactionId.ToString());
                formFields.Add("TransAmt", hdfcEnrolledCharge.Amount.ToString());

                IHtmlPostRequest htmlPostRequest = new HtmlPostRequest();
                htmlPostRequest.Method = "POST";
                htmlPostRequest.Action = $"{hdfcEnrolledCharge.AcsUrl}";
                htmlPostRequest.FormFields = formFields;
                _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                {
                    TransactionId = Convert.ToInt64(hdfcEnrolledCharge.TransactionId),
                    PaymentOptionId = PaymentOptions.CreditCard,
                    PaymentGatewayId = PaymentGateway.HDFC,
                    RequestType = "Enrolled Charge Posting",
                    Amount = hdfcEnrolledCharge.Amount.ToString(),
                    PayConfNumber = "",
                    PaymentDetail = "{\"Request\":" + JsonConvert.SerializeObject(htmlPostRequest) + "}",
                });
                return GetPaymentHtmlPostResponse(htmlPostRequest, null);
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to create html post request", ex));
                return GetPaymentHtmlPostResponse(null, ex.Message);
            }
        }

        public IPaymentResponse HdfcResponseHandler(IGatewayCharge gatewayResponse)
        {
            NameValueCollection nvcResponseQueryString = HttpUtility.ParseQueryString(gatewayResponse.QueryString);
            var paymentId = nvcResponseQueryString["MD"];
            var paymentAuthenticationResponse = nvcResponseQueryString["PaRes"];
            try
            {
                string response = gatewayResponse.Response;//HdfcChargerHelper.HttpWebRequestHandler(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Hdfc.PayerAuthenticationUrl), GetHdfcEnrolledRequest(paymentId, paymentAuthenticationResponse));
                string result = HdfcChargerHelper.GetResultField(response, "result");
                string error = HdfcChargerHelper.GetResultField(response, "error_text");
                var amount = HdfcChargerHelper.GetResultField(response, "amt");
                paymentId = HdfcChargerHelper.GetResultField(response, "paymentid");
                string paymentConfirmationNumber = HdfcChargerHelper.GetResultField(response, "tranid");
                var transactionId = HdfcChargerHelper.GetResultField(response, "trackid");
                bool payStatus = result.ToUpper().Equals("CAPTURED") ? true : false;
                if (payStatus)
                {
                    if (transactionId != null)
                    {
                        _transactionStatusUpdater.UpdateTranscationStatus(Convert.ToInt64(transactionId));
                        _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                        {
                            TransactionId = Convert.ToInt64(transactionId),
                            PaymentOptionId = PaymentOptions.CreditCard,
                            PaymentGatewayId = PaymentGateway.HDFC,
                            RequestType = "Enrolled Recieved",
                            Amount = amount.ToString(),
                            PayConfNumber = paymentConfirmationNumber,
                            PaymentDetail = "{\"QueryString\":" + JsonConvert.SerializeObject(nvcResponseQueryString.AllKeys.ToDictionary(k => k, k => nvcResponseQueryString[k])) + ",\"Response\":{\"Result\":\"" + result + "\",\"Error\":\"" + error + "\",\"Amount\":\"" + amount + "\",\"PaymentConfirmationNumber\":\"" + paymentConfirmationNumber + "\",\"PaymentId\":\"" + paymentId + "\",\"TrackId\":\"" + transactionId + "\"}}"
                        });
                    }
                }
                return GetPaymentResponse(payStatus ? true : false, payStatus ? PaymentGatewayError.None : HdfcChargerHelper.GetPaymentGatewayErrorCode(string.IsNullOrWhiteSpace(error) ? "Transaction declined" : error));
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to process transaction", ex));
                return GetPaymentResponse(false, HdfcChargerHelper.GetPaymentGatewayErrorCode(ex.Message));
            }
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