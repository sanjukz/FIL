namespace FIL.Contracts.Models.Tiqets
{
    public class TiqetProductCheckoutDetail
    {
        public long Id { get; set; }
        public string ProductId { get; set; }
        public string MustKnow { get; set; }
        public string GoodToKnow { get; set; }
        public string PrePurchase { get; set; }
        public string Usage { get; set; }
        public string Excluded { get; set; }
        public string Included { get; set; }
        public string PostPurchase { get; set; }
        public bool HasDynamicPrice { get; set; }
        public bool HasTimeSlot { get; set; }
        public bool IsEnabled { get; set; }
    }
}