using FIL.Contracts.Enums;

namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IPaymentResponse
    {
        bool Success { get; set; }
        string RedirectUrl { get; set; }
        PaymentGatewayError PaymentGatewayError { get; set; }
        PaymentGateway? PaymentGateway { get; set; }
    }

    public class PaymentResponse : IPaymentResponse
    {
        public bool Success { get; set; }
        public string RedirectUrl { get; set; }
        public PaymentGatewayError PaymentGatewayError { get; set; }
        public PaymentGateway? PaymentGateway { get; set; }
    }
}