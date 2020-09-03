using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzProductHighlight : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long EventDetailId { get; set; }
        public string Highlight { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzProductHighlightValidator : AbstractValidator<ExOzProductHighlight>, IFILValidator
    {
        public ExOzProductHighlightValidator()
        {
            RuleFor(s => s.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(s => s.Highlight).NotEmpty().WithMessage("Highlight is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}