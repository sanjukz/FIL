using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CorporateOrderInvoiceMapping : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long CorporateInvoiceDetailId { get; set; }
        public long CorporateOrderRequestId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalTicketAmount { get; set; }
        public decimal ConvenienceCharge { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal ValueAddedTax { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetTicketAmount { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CorporateOrderInvoiceMappingValidator : AbstractValidator<CorporateOrderInvoiceMapping>, IFILValidator
    {
        public CorporateOrderInvoiceMappingValidator()
        {
            RuleFor(s => s.CorporateInvoiceDetailId).NotEmpty().WithMessage("CorporateInvoiceDetailId is required");
            RuleFor(s => s.CorporateOrderRequestId).NotEmpty().WithMessage("CorporateOrderRequestId is required");
            RuleFor(s => s.Quantity).NotEmpty().WithMessage("Quantity is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}