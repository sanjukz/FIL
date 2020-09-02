using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventSponsorMapping : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long SponsorId { get; set; }
        public long EventDetailId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventSponsorMappingValidator : AbstractValidator<EventSponsorMapping>, IKzValidator
    {
        public EventSponsorMappingValidator()
        {
            RuleFor(s => s.SponsorId).NotEmpty().WithMessage("Sponsor Id is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}