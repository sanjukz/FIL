using FIL.Contracts.Enums;

namespace FIL.Contracts.Models.PaymentChargers
{
    public interface INabTransactCharge : ICharge
    {
        IPaymentCard PaymentCard { get; set; }
        IBillingAddress BillingAddress { get; set; }
        PaymentOptions PaymentOption { get; set; }
        string IPAddress { get; set; }
    }

    public class NabTransactCharge : Charge, INabTransactCharge
    {
        public IPaymentCard PaymentCard { get; set; }
        public IBillingAddress BillingAddress { get; set; }
        public PaymentOptions PaymentOption { get; set; }
        public string IPAddress { get; set; }
    }
}