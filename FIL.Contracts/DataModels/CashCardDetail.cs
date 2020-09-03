using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CashCardDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public string CardName { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CashCardDetailValidator : AbstractValidator<CashCardDetail>, IFILValidator
    {
        public CashCardDetailValidator()
        {
            RuleFor(s => s.CardName).NotEmpty().WithMessage("CardName is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}