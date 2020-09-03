using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TransactionReleaseLog : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TransactionReleaseLogValidator : AbstractValidator<TransactionReleaseLog>, IFILValidator
    {
        public TransactionReleaseLogValidator()
        {
            RuleFor(s => s.TransactionId).NotEmpty().WithMessage("TransactionId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}