using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventDeliveryTypeDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long EventDetailId { get; set; }
        public DeliveryTypes DeliveryTypeId { get; set; }
        public long RefundPolicy { get; set; }
        public string Notes { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventDeliveryTypeDetailValidator : AbstractValidator<EventDeliveryTypeDetail>, IKzValidator
    {
        public EventDeliveryTypeDetailValidator()
        {
            RuleFor(s => s.Notes).NotEmpty().WithMessage("Notes are required");
            RuleFor(s => s.EndDate).NotEmpty().WithMessage("EndDate is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}