using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventTicketDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public Guid? AltId { get; set; }
        public long EventDetailId { get; set; }
        public long TicketCategoryId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsBOEnabled { get; set; }
        public bool? IsEmailEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public InventoryType? InventoryTypeId { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventTicketDetailValidator : AbstractValidator<EventTicketDetail>, IKzValidator
    {
        public EventTicketDetailValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}