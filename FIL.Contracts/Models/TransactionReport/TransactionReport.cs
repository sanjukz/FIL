using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models.TransactionReport
{
    public class TransactionReport
    {
        public long TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public Channels Channel { get; set; }
        public string EventName { get; set; }
        public DateTime EventStartDateTime { get; set; }
        public string TicketCategory { get; set; }
        public FIL.Contracts.Enums.TicketType TicketType { get; set; }
        public string Currency { get; set; }
        public decimal PricePerTicket { get; set; }
        public int NumberOfTickets { get; set; }
        public decimal GrossTicketAmount { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}