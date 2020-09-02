using FIL.Contracts.Enums;

namespace FIL.Contracts.Models.PaymentChargers
{
    public class TransactionProvider
    {
        public long TransactionId { get; set; }
        public PaymentGateway? PaymentGateway { get; set; }
        public string Response { get; set; }
        public string Token { get; set; }
    }
}