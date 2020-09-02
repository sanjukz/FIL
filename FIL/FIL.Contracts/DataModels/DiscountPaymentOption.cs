using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class DiscountPaymentOption : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public PaymentOptions PaymentOptionId { get; set; }
        public int DiscountId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class DiscountPaymentOptionValidator : AbstractValidator<DiscountPaymentOption>, IKzValidator
    {
        public DiscountPaymentOptionValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}