using System;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Transaction
{
    public class TransactionQueryResult
    {
        public long TransactionId { get; set; }
        public Dictionary<long, bool> TransactionIds { get; set; }
        public Guid AltId { get; set; }
        public int CurrencyId { get; set; }
        public decimal? GrossTicketAmount { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public decimal? ConvenienceCharges { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? NetTicketAmount { get; set; }
        public bool IsASITicketsCreated { get; set; }
    }
}