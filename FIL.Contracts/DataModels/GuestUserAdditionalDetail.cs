using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class GuestUserAdditionalDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Guid AltId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class GuestUserAdditionalDetailsValidator : AbstractValidator<GuestUserAdditionalDetail>, IFILValidator
    {
        public GuestUserAdditionalDetailsValidator()
        {
            RuleFor(s => s.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}