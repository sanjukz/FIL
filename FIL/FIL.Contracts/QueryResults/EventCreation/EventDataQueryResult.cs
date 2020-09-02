using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.QueryResults.EventCreation
{
    public class EventDataQueryResult
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public int EventCategoryId { get; set; }
        public EventType EventTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ClientPointOfContactId { get; set; }
        public bool IsCreatedFromFeelAdmin { get; set; }
        public long? FbEventId { get; set; }
        public string MetaDetails { get; set; }
        public string TermsAndConditions { get; set; }
        public string Slug { get; set; }
        public bool IsEnabled { get; set; }
        public bool? IsDelete { get; set; }
        public bool IsTokenize { get; set; }
        public bool IsFeel { get; set; }
        public EventSource EventSourceId { get; set; }
        public bool? IsPublishedOnSite { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? PublishedDateTime { get; set; }
    }
}