using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class CorporateTransactionDetail
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public long TransactionId { get; set; }
        public long EventTicketAttributeId { get; set; }
        public long SponsorId { get; set; }
        public int TotalTickets { get; set; }
        public decimal Price { get; set; }
        public TransactingOption TransactingOptionId { get; set; }
        public bool IsEnabled { get; set; }
    }
}