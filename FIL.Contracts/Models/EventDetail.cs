using System;

namespace FIL.Contracts.Models
{
    public class EventDetail
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

        //public bool IsBOEnabled { get; set; }
        public long EventId { get; set; }

        public int VenueId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int? GroupId { get; set; }
        public string MetaDetails { get; set; }
        public bool HideEventDateTime { get; set; }
        public string CustomDateTimeMessage { get; set; }
        public string Description { get; set; }
        public int? TicketLimit { get; set; }
        public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
    }
}