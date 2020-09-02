using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class EventContainer
    {
        public string Name { get; set; }
        public Guid AltId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public List<EventDetails> EventDetails { get; set; }
    }

    public class EventDetails
    {
        public string Name { get; set; }
        public Guid AltId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}