using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class InvoiceDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public long CorporateRequestId { get; set; }
        public long EventTicketDetailId { get; set; }
        public int? TotalTickets { get; set; }
        public decimal Price { get; set; }
        public decimal? ConvenienceCharges { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? VAT { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? GrossTicketAmount { get; set; }
        public decimal? NetTicketAmount { get; set; }
        public long BankDetailsId { get; set; }
        public Guid? InvoiceGeneratedBy { get; set; }
        public DateTime? InvoiceGeneratedUtc { get; set; }
        public string InvoiceSentToEmailId { get; set; }
        public string InvoiceSentCCEmailId { get; set; }
        public string InvoiceSentBCCEmailId { get; set; }
        public DateTime? InvoiceSentDate { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class InvoiceDetailValidator : AbstractValidator<InvoiceDetail>, IFILValidator
    {
        public InvoiceDetailValidator()
        {
            RuleFor(s => s.InvoiceNumber).NotEmpty().WithMessage("InvoiceNumber is required");
            RuleFor(s => s.CorporateRequestId).NotEmpty().WithMessage("CorporateRequestId is required");
            RuleFor(s => s.EventTicketDetailId).NotEmpty().WithMessage("EventTicketDetailId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}