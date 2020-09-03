using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TransactionPaymentDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public PaymentOptions? PaymentOptionId { get; set; }
        public PaymentGateway? PaymentGatewayId { get; set; }
        public long? UserCardDetailId { get; set; }
        public string RequestType { get; set; }
        public string Amount { get; set; }
        public string PayConfNumber { get; set; }
        public string PaymentDetail { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TransactionPaymentDetailValidator : AbstractValidator<TransactionPaymentDetail>, IFILValidator
    {
        public TransactionPaymentDetailValidator()
        {
            RuleFor(s => s.RequestType).NotEmpty().WithMessage("RequestType is required");
            RuleFor(s => s.Amount).NotEmpty().WithMessage("Amount is required");
            RuleFor(s => s.PayConfNumber).NotEmpty().WithMessage("PayConfNumber is required");
            RuleFor(s => s.PaymentDetail).NotEmpty().WithMessage("PaymentDetail is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}