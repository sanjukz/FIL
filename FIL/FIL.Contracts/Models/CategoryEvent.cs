using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class CategoryEvent
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public EventCategoryData EventCategoryId { get; set; }
        public EventType EventTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ClientPointOfContactId { get; set; }
        public long? FbEventId { get; set; }
        public string MetaDetails { get; set; }
        public string TermsAndConditions { get; set; }
        public bool IsEnabled { get; set; }
        public bool? IsPublishedOnSite { get; set; }
        public DateTime? PublishedDateTime { get; set; }
    }
}