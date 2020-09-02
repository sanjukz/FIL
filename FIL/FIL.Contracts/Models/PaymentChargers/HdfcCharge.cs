namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IHdfcCharge : ICharge
    {
        IPaymentCard PaymentCard { get; set; }
    }

    public class HdfcCharge : Charge, IHdfcCharge
    {
        public IPaymentCard PaymentCard { get; set; }
    }
}