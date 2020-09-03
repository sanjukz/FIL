using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Tiqets
{
    public class TiqetProductCheckoutDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string ProductId { get; set; }
        public string MustKnow { get; set; }
        public string GoodToKnow { get; set; }
        public string PrePurchase { get; set; }
        public string Usage { get; set; }
        public string Excluded { get; set; }
        public string Included { get; set; }
        public string PostPurchase { get; set; }
        public bool HasDynamicPrice { get; set; }
        public bool HasTimeSlot { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TiqetProductCheckoutDetailValidator : AbstractValidator<TiqetProductImage>, IFILValidator
    {
        public TiqetProductCheckoutDetailValidator()
        {
            RuleFor(s => s.ProductId).NotEmpty().WithMessage("Product Id is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}