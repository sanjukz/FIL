using FIL.Configuration;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using System;
using System.Threading.Tasks;

namespace FIL.Api.PaymentChargers
{
    public interface IPaymentRedirectCharger<TP, TR> : ICharger<TP, TR>
    {
    }

    public abstract class PaymentRedirectCharger<TP> : Charger<TP, IPaymentRedirectResponse>, IPaymentRedirectCharger<TP, IPaymentRedirectResponse>
    {
        protected PaymentRedirectCharger(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public override async Task<IPaymentRedirectResponse> Charge(TP chargeParameters)
        {
            return await PaymentRedirectChargeGenerator(chargeParameters);
        }

        protected abstract Task<IPaymentRedirectResponse> PaymentRedirectChargeGenerator(TP chargeParameters);

        protected IPaymentRedirectResponse GetPaymentRedirectResponse(string uri, string error = null)
        {
            return new PaymentRedirectResponse
            {
                Uri = new Uri(uri, UriKind.Absolute),
                Error = error
            };
        }
    }
}