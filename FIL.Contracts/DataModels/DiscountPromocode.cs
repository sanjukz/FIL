using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class DiscountPromoCode : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string PromoCode { get; set; }
        public int DiscountId { get; set; }
        public PromoCodeStatus PromoCodeStatusId { get; set; }
        public bool IsEnabled { get; set; }
        public int? Limit { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class DiscountPromoCodeValidator : AbstractValidator<DiscountPromoCode>, IFILValidator
    {
        public DiscountPromoCodeValidator()
        {
            RuleFor(s => s.PromoCode).NotEmpty().WithMessage("PromoCode is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}