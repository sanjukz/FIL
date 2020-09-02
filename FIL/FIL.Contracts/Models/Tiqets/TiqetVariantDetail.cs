namespace FIL.Contracts.Models.Tiqets
{
    public class TiqetVariantDetail
    {
        public long Id { get; set; }
        public string ProductId { get; set; }
        public long VariantId { get; set; }
        public long Label { get; set; }
        public long MaxTicketsPerOrder { get; set; }
        public long DistributorCommissionExclVat { get; set; }
        public long TotalRetailPriceInclVat { get; set; }
        public long SaleTicketValueInclVat { get; set; }
        public long BookingFeeInclVat { get; set; }
        public bool DynamicVariantPricing { get; set; }
        public bool IsEnabled { get; set; }
    }
}