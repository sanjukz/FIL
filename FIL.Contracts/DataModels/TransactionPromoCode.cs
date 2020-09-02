using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TransactionPromoCode : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long TransactionId { get; set; }
        public int DiscountPromoCodeId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TransactionPromoCodeValidator : AbstractValidator<TransactionPromoCode>, IKzValidator
    {
        public TransactionPromoCodeValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}