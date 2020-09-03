using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ReportingColumn : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public string DBFieldName { get; set; }
        public string DisplayName { get; set; }
        public bool IsEnabled { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ReportingColumnValidator : AbstractValidator<ReportingColumn>, IFILValidator
    {
        public ReportingColumnValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}