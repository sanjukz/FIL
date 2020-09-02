using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TournamentSectionFeeDetail : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long TournamentLayoutSectionId { get; set; }
        public short FeeId { get; set; }
        public string DisplayName { get; set; }
        public short ValueTypeId { get; set; }
        public decimal Value { get; set; }
        public short? FeeGroupId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TournamentSectionFeeDetailValidator : AbstractValidator<TournamentSectionFeeDetail>, IKzValidator
    {
        public TournamentSectionFeeDetailValidator()
        {
            RuleFor(s => s.Value).NotEmpty().WithMessage("Value is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}