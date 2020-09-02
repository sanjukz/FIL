using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class DiscountCustomer : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public DiscountCustomersTypes DiscountCustomersTypeId { get; set; }
        public int DiscountId { get; set; }
        public long? TransactionId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class DiscountCustomerValidator : AbstractValidator<DiscountCustomer>, IKzValidator
    {
        public DiscountCustomerValidator()
        {
            RuleFor(s => s.CustomerEmail).NotEmpty().WithMessage("CustomerEmail is required");
            RuleFor(s => s.CustomerPhone).NotEmpty().WithMessage("CustomerPhone is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}