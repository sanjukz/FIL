namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IStripeCharge : ICharge
    {
        string Token { get; set; }
        string RedirectUrl { get; set; }
        bool IsIntentConfirm { get; set; }
    }

    public class StripeCharge : Charge, IStripeCharge
    {
        public string Token { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsIntentConfirm { get; set; }
    }
}