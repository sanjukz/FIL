using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CountryDescription : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public int CountryId { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CountryDescriptionValidator : AbstractValidator<CountryDescription>, IKzValidator
    {
        public CountryDescriptionValidator()
        {
            RuleFor(s => s.CountryId).NotEmpty().WithMessage("Country is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}