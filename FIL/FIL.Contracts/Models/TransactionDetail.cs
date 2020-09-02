using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class TransactionDetail
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public long EventTicketAttributeId { get; set; }
        public short TotalTickets { get; set; }
        public decimal PricePerTicket { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public decimal? ConvenienceCharges { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? DiscountAmount { get; set; }
        public short? TicketTypeId { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime VisitEndDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public bool? IsRedeemed { get; set; }
    }
}