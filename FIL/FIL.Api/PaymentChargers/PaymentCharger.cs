using FIL.Configuration;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using System.Threading.Tasks;

namespace FIL.Api.PaymentChargers
{
    public interface IPaymentCharger<TP, TR> : ICharger<TP, TR>
    {
    }

    public abstract class PaymentCharger<TP> : Charger<TP, IPaymentResponse>, IPaymentCharger<TP, IPaymentResponse>
    {
        protected PaymentCharger(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public override async Task<IPaymentResponse> Charge(TP chargeParameters)
        {
            return await CreateCharge(chargeParameters);
        }

        protected abstract Task<IPaymentResponse> CreateCharge(TP chargeParameters);
    }
}