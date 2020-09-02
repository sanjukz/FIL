using FIL.Configuration;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Threading.Tasks;

namespace FIL.Api.PaymentChargers
{
    public interface IFreeCharger<ICharge, IPaymentResponse> : IPaymentCharger<ICharge, IPaymentResponse>
    {
    }

    public class FreeCharger : PaymentCharger<ICharge>, IFreeCharger<ICharge, IPaymentResponse>
    {
        public FreeCharger(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        protected override async Task<IPaymentResponse> CreateCharge(ICharge charge)
        {
            try
            {
                if (Math.Ceiling(charge.Amount) == 0)
                {
                    return GetPaymentResponse(true, PaymentGatewayError.None);
                }
                else
                {
                    return GetPaymentResponse(false, PaymentGatewayError.InvalidAmount);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to create charge", ex));
                return GetPaymentResponse(false, PaymentGatewayError.Unknown);
            }
        }
    }
}