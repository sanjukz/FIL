using FIL.Configuration;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using System.Threading.Tasks;

namespace FIL.Api.PaymentChargers
{
    public interface IPaymentHtmlPostCharger<TP, TR> : ICharger<TP, TR>
    {
    }

    public abstract class PaymentHtmlPostCharger<TP> : Charger<TP, IPaymentHtmlPostResponse>, IPaymentHtmlPostCharger<TP, IPaymentHtmlPostResponse>
    {
        protected PaymentHtmlPostCharger(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public override async Task<IPaymentHtmlPostResponse> Charge(TP chargeParameters)
        {
            return await PaymentHtmlPostChargeGenerator(chargeParameters);
        }

        protected abstract Task<IPaymentHtmlPostResponse> PaymentHtmlPostChargeGenerator(TP chargeParameters);

        protected IPaymentHtmlPostResponse GetPaymentHtmlPostResponse(IHtmlPostRequest htmlPostRequest, string error = null)
        {
            return new PaymentHtmlPostResponse
            {
                HtmlPostRequest = htmlPostRequest,
                Error = error
            };
        }
    }
}