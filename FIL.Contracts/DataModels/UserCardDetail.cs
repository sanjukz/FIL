using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class UserCardDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Guid AltId { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public short? ExpiryMonth { get; set; }
        public short? ExpiryYear { get; set; }
        public CardType? CardTypeId { get; set; }
        public bool? IsDefault { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class UserCardDetailValidator : AbstractValidator<UserCardDetail>, IKzValidator
    {
        public UserCardDetailValidator()
        {
            RuleFor(s => s.NameOnCard).NotEmpty().WithMessage("NameOnCard is required");
            RuleFor(s => s.CardNumber).NotEmpty().WithMessage("CardNumber is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}