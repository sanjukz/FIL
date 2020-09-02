using System;

namespace FIL.Contracts.Models
{
    public class EventHostMapping
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string MetaDetails { get; set; }
        public long EventId { get; set; }
        public bool IsEnabled { get; set; }
    }
}