using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzTransaction : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public long VoucherId { get; set; }
        public long PurchaserId { get; set; }
        public string ReferenceNo { get; set; }
        public long ProductId { get; set; }
        public decimal SoldPrice { get; set; }
        public decimal SoldPriceGST { get; set; }
        public decimal AgentShare { get; set; }
        public decimal AgentShareGST { get; set; }
        public long VoucherDetailsId { get; set; }
        public string Notes { get; set; }
        public long PickupDetailsId { get; set; }
        public long DropOffDetailsId { get; set; }
        public string MetaData { get; set; }
        public string TicketLink { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzTransactionValidator : AbstractValidator<ExOzTransaction>, IFILValidator
    {
        public ExOzTransactionValidator()
        {
            RuleFor(s => s.TransactionId).NotEmpty().WithMessage("TransactionId is required");
            RuleFor(s => s.PurchaserId).NotEmpty().WithMessage("PurchaserId is required");
            RuleFor(s => s.ReferenceNo).NotEmpty().WithMessage("ReferenceNo is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}