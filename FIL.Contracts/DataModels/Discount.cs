using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class Discount : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DiscountTypes DiscountTypeId { get; set; }
        public string Description { get; set; }
        public DiscountValueType DiscountValueTypeId { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal? MaximumDiscountPerTransaction { get; set; }
        public short? MinimumQuantityPerTransaction { get; set; }
        public short? MaximumQuantityPerTransaction { get; set; }
        public decimal? OverallMaximumDiscount { get; set; }
        public short? OverallMaximumQuantity { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class DiscountValidator : AbstractValidator<Discount>, IKzValidator
    {
        public DiscountValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(s => s.DiscountValue).NotEmpty().WithMessage("DiscountValue is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}