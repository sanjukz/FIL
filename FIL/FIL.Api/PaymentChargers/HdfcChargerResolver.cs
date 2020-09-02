using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;

namespace FIL.Api.PaymentChargers
{
    public interface IHdfcChargerResolver
    {
        IHdfcEnrollmentResponse HdfcEnrollmentVerification(IHdfcCharge hdfcCharge);
    }

    public class HdfcChargerResolver : IHdfcChargerResolver
    {
        protected readonly ILogger _logger;
        protected readonly ISettings _settings;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;

        public HdfcChargerResolver(ILogger logger, ISettings settings, ITransactionPaymentDetailRepository transactionPaymentDetailRepository)
        {
            _logger = logger;
            _settings = settings;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
        }

        public IHdfcEnrollmentResponse HdfcEnrollmentVerification(IHdfcCharge hdfcCharge)
        {
            try
            {
                IHdfcEnrolledCharge hdfcEnrolledCharge = new HdfcEnrolledCharge();
                string response = HdfcChargerHelper.HttpWebRequestHandler(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Hdfc.EnrollmentVerificationUrl), HdfcChargerHelper.GetHdfcChargeRequest(hdfcCharge, _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Hdfc.TranportalId), _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Hdfc.TranportalPassword)));
                string result = HdfcChargerHelper.GetResultField(response, "result");
                string error = HdfcChargerHelper.GetResultField(response, "error_text");
                bool errorStatus = string.IsNullOrWhiteSpace(error) ? true : false;
                if (errorStatus)
                {
                    if (result.ToUpper().Equals("ENROLLED"))
                    {
                        hdfcEnrolledCharge.AcsUrl = HdfcChargerHelper.GetResultField(response, "url");
                        hdfcEnrolledCharge.PaymentAuthenticationRequest = HdfcChargerHelper.GetResultField(response, "PAReq");
                        hdfcEnrolledCharge.PaymentId = HdfcChargerHelper.GetResultField(response, "paymentid");
                    }
                    hdfcEnrolledCharge.Result = HdfcChargerHelper.GetResultField(response, "result");
                    hdfcEnrolledCharge.Error = HdfcChargerHelper.GetResultField(response, "error_text");
                    hdfcEnrolledCharge.Eci = !string.IsNullOrWhiteSpace(HdfcChargerHelper.GetResultField(response, "eci")) ? HdfcChargerHelper.GetResultField(response, "eci") : "7";
                    hdfcEnrolledCharge.TransactionId = hdfcCharge.TransactionId;
                    hdfcEnrolledCharge.Amount = hdfcCharge.Amount;
                    _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                    {
                        TransactionId = Convert.ToInt64(hdfcCharge.TransactionId),
                        PaymentOptionId = PaymentOptions.CreditCard,
                        PaymentGatewayId = PaymentGateway.HDFC,
                        UserCardDetailId = hdfcCharge.UserCardDetailId,
                        RequestType = "Charge Resolver",
                        Amount = hdfcCharge.Amount.ToString(),
                        PayConfNumber = "",
                        PaymentDetail = "{\"Response\":" + Newtonsoft.Json.JsonConvert.SerializeObject(hdfcEnrolledCharge) + "}",
                    });
                }
                return GetHdfcEnrollmentResponse(errorStatus ? hdfcEnrolledCharge : null, errorStatus ? PaymentGatewayError.None : HdfcChargerHelper.GetPaymentGatewayErrorCode(string.IsNullOrWhiteSpace(error) ? "Transaction declined" : error));
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to verify enrollment", ex));
                return GetHdfcEnrollmentResponse(null, HdfcChargerHelper.GetPaymentGatewayErrorCode(ex.Message));
            }
        }

        protected IHdfcEnrollmentResponse GetHdfcEnrollmentResponse(IHdfcEnrolledCharge hdfcEnrolledCharge, PaymentGatewayError paymentGatewayError = PaymentGatewayError.None)
        {
            return new HdfcEnrollmentResponse
            {
                HdfcEnrolledCharge = hdfcEnrolledCharge,
                PaymentGatewayError = paymentGatewayError
            };
        }
    }
}