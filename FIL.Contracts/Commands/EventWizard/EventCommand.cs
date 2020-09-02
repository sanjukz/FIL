using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Commands.EventWizard
{
    public class EventCommand : BaseCommand
    {
        public Int16 EventCategoryId { get; set; }
        public EventType EventTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ClientPointOfContactId { get; set; }
        public long? FbEventId { get; set; }
        public string MetaDetails { get; set; }
        public string TermsAndConditions { get; set; }
        public bool? IsPublishedOnSite { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public int? PublishedBy { get; set; }
        public int? TestedBy { get; set; }
    }
}