using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventHistory : Event
    {
    }

    public class Event : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public int EventCategoryId { get; set; }
        public EventType EventTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ClientPointOfContactId { get; set; }
        public long? FbEventId { get; set; }
        public string MetaDetails { get; set; }
        public bool IsFeel { get; set; }
        public bool IsCreatedFromFeelAdmin { get; set; }
        public EventSource EventSourceId { get; set; }
        public string TermsAndConditions { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsTokenize { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsPublishedOnSite { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public int? PublishedBy { get; set; }
        public int? TestedBy { get; set; }
        public string Slug { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventTypeId { get; set; }
        public FIL.Contracts.Enums.EventStatus EventStatusId { get; set; }
        public string Url { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventValidator : AbstractValidator<Event>, IKzValidator
    {
        public EventValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(s => s.TermsAndConditions).NotEmpty().WithMessage("TermsAndConditions are required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}