using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class OfflineCustomer : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long TransactionId { get; set; }
        public PaymentType PaymentTypeId { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }
        public bool IsEnabled { get; set; }
        public Modules ModuleId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class OfflineCustomerValidator : AbstractValidator<OfflineCustomer>, IKzValidator
    {
        public OfflineCustomerValidator()
        {
            RuleFor(s => s.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(s => s.TransactionId).NotEmpty().WithMessage("TransactionId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}