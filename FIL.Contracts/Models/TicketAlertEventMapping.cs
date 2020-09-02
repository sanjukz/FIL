using System;

namespace FIL.Contracts.Models
{
    public class TicketAlertEventMapping
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public long EventId { get; set; }
        public long EventDetailId { get; set; }
        public int CountryId { get; set; }
        public bool IsEnabled { get; set; }
    }
}