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
using System.Threading.Tasks;

namespace FIL.Api.PaymentChargers
{
    public interface IHdfcNotEnrolledCharger<IHdfcCharge, IPaymentResponse> : IPaymentCharger<IHdfcCharge, IPaymentResponse>
    {
    }

    public class HdfcNotEnrolledCharger : PaymentCharger<IHdfcCharge>, IHdfcNotEnrolledCharger<IHdfcCharge, IPaymentResponse>
    {
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly ITransactionStatusUpdater _transactionStatusUpdater;

        public HdfcNotEnrolledCharger(ILogger logger, ISettings settings,
             ITransactionPaymentDetailRepository transactionPaymentDetailRepository, ITransactionStatusUpdater transactionStatusUpdater)
            : base(logger, settings)
        {
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _transactionStatusUpdater = transactionStatusUpdater;
        }

        protected override async Task<IPaymentResponse> CreateCharge(IHdfcCharge hdfcCharge)
        {
            try
            {
                string response = HdfcChargerHelper.HttpWebRequestHandler(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Hdfc.TransactionPortalUrl), HdfcChargerHelper.GetHdfcChargeRequest(hdfcCharge, _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Hdfc.TranportalId), _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Hdfc.TranportalPassword)));
                string result = HdfcChargerHelper.GetResultField(response, "result");
                string error = HdfcChargerHelper.GetResultField(response, "error_text");
                string amount = HdfcChargerHelper.GetResultField(response, "amt");
                string paymentId = HdfcChargerHelper.GetResultField(response, "payid");
                string transactionId = HdfcChargerHelper.GetResultField(response, "trackid");
                string paymentConfirmationNumber = HdfcChargerHelper.GetResultField(response, "tranid");
                bool payStatus = result.ToUpper().Equals("CAPTURED") ? true : false;
                if (payStatus)
                {
                    if (transactionId != null)
                    {
                        _transactionStatusUpdater.UpdateTranscationStatus(Convert.ToInt64(transactionId));
                    }
                }
                _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                {
                    TransactionId = Convert.ToInt64(hdfcCharge.TransactionId),
                    PaymentOptionId = PaymentOptions.CreditCard,
                    PaymentGatewayId = PaymentGateway.HDFC,
                    RequestType = "Not Enrolled Recieved",
                    Amount = hdfcCharge.Amount.ToString(),
                    PayConfNumber = paymentConfirmationNumber,
                    PaymentDetail = "{\"Response\":{\"Result\":\"" + result + "\",\"Error\":\"" + error + "\",\"Amount\":\"" + amount + "\",\"PaymentConfirmationNumber\":\"" + paymentConfirmationNumber + "\",\"PaymentId\":\"" + paymentId + "\",\"TrackId\":\"" + transactionId + "\"}",
                });
                return GetPaymentResponse(payStatus ? true : false, payStatus ? PaymentGatewayError.None : HdfcChargerHelper.GetPaymentGatewayErrorCode(string.IsNullOrWhiteSpace(error) ? "Transaction declined" : error));
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to create charge", ex));
                return GetPaymentResponse(false, HdfcChargerHelper.GetPaymentGatewayErrorCode(ex.Message));
            }
        }
    }
}