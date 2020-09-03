using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class DomesticCardBin : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int FirstSix { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class DomesticCardBinValidator : AbstractValidator<DomesticCardBin>, IFILValidator
    {
        public DomesticCardBinValidator()
        {
            RuleFor(s => s.FirstSix).NotEmpty().WithMessage("FirstSix are required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}