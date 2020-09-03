using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzProductImage : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long EventDetailId { get; set; }
        public string ImageURL { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzProductImageValidator : AbstractValidator<ExOzProductImage>, IFILValidator
    {
        public ExOzProductImageValidator()
        {
            RuleFor(s => s.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(s => s.ImageURL).NotEmpty().WithMessage("ImageURL is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}