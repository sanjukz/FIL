using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventTicketDiscountDetail : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long EventTicketAttributeId { get; set; }
        public int DiscountId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventTicketDiscountDetailValidator : AbstractValidator<EventTicketDiscountDetail>, IKzValidator
    {
        public EventTicketDiscountDetailValidator()
        {
            RuleFor(s => s.StartDateTime).NotEmpty().WithMessage("StartDateTime is required");
            RuleFor(s => s.EndDateTime).NotEmpty().WithMessage("EndDateTime is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}