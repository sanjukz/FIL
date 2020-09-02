using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class RefillWalletDetail : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal? WalletAmount { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class RefillWalletDetailValidator : AbstractValidator<RefillWalletDetail>, IKzValidator
    {
        public RefillWalletDetailValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}