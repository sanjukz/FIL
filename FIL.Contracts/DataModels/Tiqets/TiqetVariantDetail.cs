using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Tiqets
{
    public class TiqetVariantDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string ProductId { get; set; }
        public long VariantId { get; set; }
        public string Label { get; set; }
        public long MaxTicketsPerOrder { get; set; }
        public decimal DistributorCommissionExclVat { get; set; }
        public decimal TotalRetailPriceInclVat { get; set; }
        public decimal SaleTicketValueInclVat { get; set; }
        public decimal BookingFeeInclVat { get; set; }
        public bool DynamicVariantPricing { get; set; }
        public string ValidWithVariantIds { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TiqetVariantDetailValidator : AbstractValidator<TiqetVariantDetail>, IFILValidator
    {
        public TiqetVariantDetailValidator()
        {
            RuleFor(s => s.ProductId).NotEmpty().WithMessage("Product Id is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}