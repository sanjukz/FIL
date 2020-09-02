using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Events
{
    public class EventsFeelQueryResult
    {
        public List<Event> Events { get; set; }
    }

    public class Event
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public int EventCategoryId { get; set; }
        public EventType EventTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsFeel { get; set; }
        public EventSource EventSourceId { get; set; }
        public bool? IsPublishedOnSite { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public string ImagePath { get; set; }
    }
}