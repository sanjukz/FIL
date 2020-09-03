using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Tiqets
{
    public class TiqetsTransaction : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string OrderReferenceId { get; set; }
        public long TransactionId { get; set; }
        public bool IsOrderConfirmed { get; set; }
        public string PaymentConfirmationToken { get; set; }
        public int OrderStatus { get; set; }
        public string TicketPdf { get; set; }
        public string HowToUse { get; set; }
        public string PostPurchaseInfo { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TiqetsTransactionValidator : AbstractValidator<TiqetsTransaction>, IFILValidator
    {
        public TiqetsTransactionValidator()
        {
            RuleFor(s => s.OrderReferenceId).NotEmpty().WithMessage("Order Refernce is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}