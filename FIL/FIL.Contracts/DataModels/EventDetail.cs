using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public long EventId { get; set; }
        public int VenueId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public long? GroupId { get; set; }
        public string MetaDetails { get; set; }
        public string Description { get; set; }
        public int? TicketLimit { get; set; }
        public bool IsEnabled { get; set; }
        public bool HideEventDateTime { get; set; }
        public string CustomDateTimeMessage { get; set; }
        public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventDetailValidator : AbstractValidator<EventDetail>, IKzValidator
    {
        public EventDetailValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(s => s.StartDateTime).NotEmpty().WithMessage("StartDateTime is required");
            RuleFor(s => s.EndDateTime).NotEmpty().WithMessage("EndDateTime is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}