using FIL.Contracts.Enums;

namespace FIL.Contracts.Models.PaymentChargers
{
    public interface ICharge
    {
        long TransactionId { get; set; }
        decimal Amount { get; set; }
        string Currency { get; set; }
        long UserCardDetailId { get; set; }
        Channels? ChannelId { get; set; }
        User User { get; set; }
    }

    public class Charge : ICharge
    {
        public long TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public long UserCardDetailId { get; set; }
        public Channels? ChannelId { get; set; }
        public User User { get; set; }
    }
}