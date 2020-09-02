using System;

namespace FIL.Contracts.Models.CreateEventV1
{
    public class TicketModel
    {
        public long ETDId { get; set; }
        public Guid? TicketAltId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TicketCategoryNotes { get; set; }
        public int TicketCategoryId { get; set; }
        public int TicketCategoryTypeId { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public int TotalQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public int Quantity { get; set; }
        public bool isEnabled { get; set; }
        public decimal Price { get; set; }
        public string PromoCode { get; set; }
        public FIL.Contracts.Enums.DiscountValueType DiscountValueType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool IsDiscountEnable { get; set; }
        public decimal? DonationAmount1 { get; set; }
        public decimal? DonationAmount2 { get; set; }
        public decimal? DonationAmount3 { get; set; }
    }
}