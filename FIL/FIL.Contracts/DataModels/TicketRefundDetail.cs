using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TicketRefundDetail : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public string BarcodeNumber { get; set; }
        public long TransactionId { get; set; }
        public decimal? RefundedAmount { get; set; }
        public bool IsEnabled { get; set; }
        public long? ExchangedId { get; set; }
        public bool? IsExchanged { get; set; }
        public decimal? ExchangedAmount { get; set; }
        public short? ActionTypeId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TicketRefundDetailValidator : AbstractValidator<TicketRefundDetail>, IKzValidator
    {
        public TicketRefundDetailValidator()
        {
            RuleFor(s => s.BarcodeNumber).NotEmpty().WithMessage("BarcodeNumber is required");
            RuleFor(s => s.TransactionId).NotEmpty().WithMessage("TransactionId is required");
            RuleFor(s => s.RefundedAmount).NotEmpty().WithMessage("RefundedAmount is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}