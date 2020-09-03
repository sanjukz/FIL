using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class BoCustomerDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public long TransactionId { get; set; }
        public string PaymentMode { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public bool IsEnabled { get; set; }
        public decimal Value { get; set; }
        public int CurrencyId { get; set; }
        public string ZipCode { get; set; }
        public int PaymentOptionId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }

        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class BoCustomerDetailValidator : AbstractValidator<BoCustomerDetail>, IFILValidator
    {
        public BoCustomerDetailValidator()
        {
            RuleFor(s => s.PaymentMode).NotEmpty().WithMessage("PaymentMode is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}