using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzVoucherDetail : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long VoucherId { get; set; }
        public long TransactionId { get; set; }
        public long Quantity { get; set; }
        public long ProductOption { get; set; }
        public string OptionName { get; set; }
        public decimal OptionUnitPrice { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzVoucherDetailValidator : AbstractValidator<ExOzVoucherDetail>, IFILValidator
    {
        public ExOzVoucherDetailValidator()
        {
            RuleFor(s => s.VoucherId).NotEmpty().WithMessage("VoucherId is required");
            RuleFor(s => s.TransactionId).NotEmpty().WithMessage("TransactionId is required");
            RuleFor(s => s.ProductOption).NotEmpty().WithMessage("ProductOption is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}