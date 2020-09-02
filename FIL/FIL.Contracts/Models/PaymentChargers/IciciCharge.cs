namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IIciciCharge : ICharge
    {
        IPaymentCard PaymentCard { get; set; }
    }

    public class IciciCharge : Charge, IIciciCharge
    {
        public IPaymentCard PaymentCard { get; set; }
    }
}