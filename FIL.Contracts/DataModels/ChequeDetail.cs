using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ChequeDetail : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public long TransactionId { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ChequeDetailValidator : AbstractValidator<ChequeDetail>, IKzValidator
    {
        public ChequeDetailValidator()
        {
            RuleFor(s => s.BankName).NotEmpty().WithMessage("BankName is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}