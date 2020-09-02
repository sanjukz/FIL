using System;

namespace FIL.Contracts.Models.CreateEventV1
{
    public class DiscountModel
    {
        public long EventTicketAttributeId { get; set; }
        public string PromoCode { get; set; }
        public FIL.Contracts.Enums.DiscountTypes DiscountTypes { get; set; }
        public FIL.Contracts.Enums.DiscountValueType DiscountValueType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string EventName { get; set; }
        public bool IsEnabled { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}