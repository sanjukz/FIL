using System;

namespace FIL.Contracts.Models
{
    public class CorporateTicketAllocationDetail
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public long EventTicketAttributeId { get; set; }
        public long SponsorId { get; set; }
        public int AllocatedTickets { get; set; }
        public int RemainingTickets { get; set; }
        public decimal Price { get; set; }
        public bool IsCorporateRequest { get; set; }
        public long CorporateRequestId { get; set; }
        public bool IsEnabled { get; set; }
    }
}