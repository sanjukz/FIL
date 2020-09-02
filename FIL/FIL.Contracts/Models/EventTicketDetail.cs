using System;

namespace FIL.Contracts.Models
{
    public class EventTicketDetail
    {
        public long Id { get; set; }
        public Guid? AltId { get; set; }
        public long EventDetailId { get; set; }
        public long TicketCategoryId { get; set; }
    }
}