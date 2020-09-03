using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class Keyword : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string Keywords { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class KeywordValidator : AbstractValidator<Keyword>, IFILValidator
    {
        public KeywordValidator()
        {
            RuleFor(s => s.Keywords).NotEmpty().WithMessage("Keywords are required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}