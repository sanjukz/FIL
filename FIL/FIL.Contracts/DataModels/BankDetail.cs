using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class BankDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string BankDetails { get; set; }
        public int CountryId { get; set; }
        public bool IsEnabled { get; set; }
        public bool? IsIntermediaryBank { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class BankDetailValidator : AbstractValidator<BankDetail>, IKzValidator
    {
        public BankDetailValidator()
        {
            RuleFor(s => s.BankDetails).NotEmpty().WithMessage("BankDetails is required");
            RuleFor(s => s.CountryId).NotEmpty().WithMessage("CountryId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}