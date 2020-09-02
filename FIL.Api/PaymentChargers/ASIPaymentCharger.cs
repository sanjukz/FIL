using FIL.Api.Providers.Transaction;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Threading.Tasks;

namespace FIL.Api.PaymentChargers
{
    public interface IASIPaymentCharger<IASITransactCharge, IPaymentResponse> : IPaymentCharger<IASITransactCharge, IPaymentResponse>
    {
    }

    public class ASIPaymentCharger : PaymentCharger<IASITransactCharge>, IASIPaymentCharger<IASITransactCharge, IPaymentResponse>
    {
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionStatusUpdater _transactionStatusUpdater;

        public ASIPaymentCharger(ILogger logger,
            ISettings settings,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            ITransactionRepository transactionRepository, ITransactionStatusUpdater transactionStatusUpdater
           )
            : base(logger, settings)
        {
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _transactionRepository = transactionRepository;
            _transactionStatusUpdater = transactionStatusUpdater;
        }

        protected override async Task<IPaymentResponse> CreateCharge(IASITransactCharge asiChargeParameters)
        {
            try
            {
                try
                {
                    var PaymentDetail = "{\"Response\":" + Newtonsoft.Json.JsonConvert.SerializeObject(asiChargeParameters.asiFormData) + "}";
                    _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                    {
                        TransactionId = Convert.ToInt64(asiChargeParameters.asiFormData.Data.TransactionId),
                        PaymentOptionId = PaymentOptions.None,
                        PaymentGatewayId = PaymentGateway.Payu,
                        RequestType = "Charge Recieved",
                        Amount = asiChargeParameters.asiFormData.Data.Payment.Amount.ToString(),
                        PayConfNumber = asiChargeParameters.asiFormData.Data.Payment.Id,
                        PaymentDetail = PaymentDetail,
                    });
                }
                catch (Exception e)
                {
                    _logger.Log(LogCategory.Error, new Exception("Failed to generate html post request", e));
                }
                _transactionStatusUpdater.UpdateTranscationStatus(Convert.ToInt64(asiChargeParameters.asiFormData.Data.TransactionId));
                return GetPaymentResponse(true, PaymentGatewayError.None, PaymentGateway.Payu);
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to Create Charge of ASI Response", ex));
                return GetPaymentResponse(false, GetPaymentGatewayErrorCode(asiChargeParameters.asiFormData.Error), PaymentGateway.Payu);
            }
        }

        private PaymentGatewayError GetPaymentGatewayErrorCode(string errorCode)
        {
            string errorMessage = string.Empty;
            switch (errorCode)
            {
                case "TV01": errorMessage = "TransactionId does not exists"; return PaymentGatewayError.TransactionDeclined;
                case "TV02": errorMessage = "Invalid hash"; return PaymentGatewayError.InvalidMerchantId;
                case "TV05": errorMessage = "Vendor disabled"; return PaymentGatewayError.DoNotHonour;
                case "TP01": errorMessage = "Payment canceled"; return PaymentGatewayError.TransactionCancelled;
                case "TP02": errorMessage = "payment failed"; return PaymentGatewayError.InvalidAmount;
                case "TP03": errorMessage = "Refund initiated"; return PaymentGatewayError.InvalidCardNumber;
                default: return PaymentGatewayError.Unknown;
            }
        }
    }
}