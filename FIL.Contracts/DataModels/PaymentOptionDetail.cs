using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class PaymentOptionDetail : IId<int>, IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short PaymentOptionId { get; set; }
        public bool? IsFrequentlyUsed { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class PaymentOptionDetailValidator : AbstractValidator<PaymentOptionDetail>, IFILValidator
    {
        public PaymentOptionDetailValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}