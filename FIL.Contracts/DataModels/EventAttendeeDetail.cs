using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventAttendeeDetail : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public long? TransactionDetailId { get; set; }
        public long EventFormFieldId { get; set; }
        public string Value { get; set; }
        public short AttendeeNumber { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventAttendeeDetailValidator : AbstractValidator<EventAttendeeDetail>, IFILValidator
    {
        public EventAttendeeDetailValidator()
        {
            RuleFor(s => s.Value).NotEmpty().WithMessage("Value is required");
            RuleFor(s => s.AttendeeNumber).NotEmpty().WithMessage("AttendeeNumber is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}