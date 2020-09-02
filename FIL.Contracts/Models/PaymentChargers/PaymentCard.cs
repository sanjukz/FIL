using FIL.Contracts.Enums;

namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IPaymentCard
    {
        string CardNumber { get; set; }
        string NameOnCard { get; set; }
        string Cvv { get; set; }
        short ExpiryMonth { get; set; }
        short ExpiryYear { get; set; }
        CardType CardType { get; set; }
    }

    public class PaymentCard : IPaymentCard
    {
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public string Cvv { get; set; }
        public short ExpiryMonth { get; set; }
        public short ExpiryYear { get; set; }
        public CardType CardType { get; set; }
    }
}