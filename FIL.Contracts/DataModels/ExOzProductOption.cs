using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzProductOption : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long ProductOptionId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long? SessionId { get; set; }
        public long EventTicketDetailId { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? MaxQty { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? DefaultQty { get; set; }
        public decimal? Multiple { get; set; }
        public decimal? Weight { get; set; }
        public bool? IsFromPrice { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzProductOptionValidator : AbstractValidator<ExOzProductOption>, IFILValidator
    {
        public ExOzProductOptionValidator()
        {
            RuleFor(s => s.ProductOptionId).NotEmpty().WithMessage("ProductOptionId is required");
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(s => s.SessionId).NotEmpty().WithMessage("SessionId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}