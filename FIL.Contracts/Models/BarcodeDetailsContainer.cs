using System;

namespace FIL.Contracts.Models
{
    public class BarcodeDetailsContainer
    {
        public Guid AltId { get; set; }
        public string BarcodeNumber { get; set; }
        public long TicketCategoryId { get; set; }
        public string TicketCategory { get; set; }
        public decimal Value { get; set; }
        public string CurrencyCode { get; set; }
        public string Postalcode { get; set; }
        public bool IsConsumed { get; set; }
        public DateTime? ConsumedDateTime { get; set; }
        public DateTime TransactionUTC { get; set; }
    }
}