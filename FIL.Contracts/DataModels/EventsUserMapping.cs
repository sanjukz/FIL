using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventsUserMapping : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long EventId { get; set; }
        public long EventDetailId { get; set; }
        public bool IsEnabled { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventsUserMappingValidator : AbstractValidator<EventsUserMapping>, IFILValidator
    {
        public EventsUserMappingValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}