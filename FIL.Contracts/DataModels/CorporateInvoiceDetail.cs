using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CorporateInvoiceDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public int CurrencyId { get; set; }
        public int CompanyDetailId { get; set; }
        public long BankDetailId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CorporateInvoiceDetailValidator : AbstractValidator<CorporateInvoiceDetail>, IFILValidator
    {
        public CorporateInvoiceDetailValidator()
        {
            RuleFor(s => s.CurrencyId).NotEmpty().WithMessage("CurrencyId is required");
            RuleFor(s => s.InvoiceDate).NotEmpty().WithMessage("InvoiceDate is required");
            RuleFor(s => s.InvoiceDueDate).NotEmpty().WithMessage("InvoiceDueDate is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}