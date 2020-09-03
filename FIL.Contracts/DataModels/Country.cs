using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class Country : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string IsoAlphaTwoCode { get; set; }
        public string IsoAlphaThreeCode { get; set; }
        public string CountryName { get; set; }
        public int? Numcode { get; set; }
        public int? Phonecode { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CountryValidator : AbstractValidator<Country>, IFILValidator
    {
        public CountryValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(s => s.IsoAlphaTwoCode).NotEmpty().WithMessage("ISO Alpha-2 code is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}