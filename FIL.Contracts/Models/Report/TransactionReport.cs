using System;

namespace FIL.Contracts.Models.Report
{
    public class TransactionReport
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string Channels { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string EventName { get; set; }
        public string EventDetailName { get; set; }
        public string CategotyName { get; set; }
        public short TotalTickets { get; set; }
        public decimal PricePerTicket { get; set; }
        public decimal GrossTicketAmount { get; set; }
        public decimal ConvenienceCharges { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetTicketAmount { get; set; }
    }
}