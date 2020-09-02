using FIL.Configuration;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using System.Threading.Tasks;

namespace FIL.Api.PaymentChargers
{
    public interface ICharger<TP, TR>
    {
        Task<TR> Charge(TP charge);
    }

    public abstract class Charger<TP, TR> : ICharger<TP, TR>
    {
        protected readonly ILogger _logger;
        protected readonly ISettings _settings;

        protected Charger(ILogger logger, ISettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public abstract Task<TR> Charge(TP charge);

        protected IPaymentResponse GetPaymentResponse(bool success = false, PaymentGatewayError paymentGatewayError = PaymentGatewayError.None, PaymentGateway PaymentGateway = PaymentGateway.None, string ClientSecret = "")
        {
            return new PaymentResponse
            {
                Success = success,
                PaymentGatewayError = paymentGatewayError,
                PaymentGateway = PaymentGateway,
                RedirectUrl = ClientSecret
            };
        }
    }
}