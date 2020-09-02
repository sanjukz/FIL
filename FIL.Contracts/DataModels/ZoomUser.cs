using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ZoomUser : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public int RoleId { get; set; }
        public long UserId { get; set; }
        public long EventId { get; set; }
        public int EventHostUserId { get; set; }
        public string UniqueId { get; set; }
        public bool IsActive { get; set; }
        public bool IsEnabled { get; set; }
        public long TransactionId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ZoomUserValidator : AbstractValidator<ZoomUser>, IKzValidator
    {
        public ZoomUserValidator()
        {
            RuleFor(s => s.RoleId).NotEmpty().WithMessage("Role Id is required");
            RuleFor(s => s.EventId).NotEmpty().WithMessage("Event Id required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}