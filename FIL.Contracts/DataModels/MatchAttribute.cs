using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class MatchAttribute : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long EventDetailId { get; set; }
        public long TeamA { get; set; }
        public long TeamB { get; set; }
        public int MatchNo { get; set; }
        public int MatchDay { get; set; }
        public DateTime MatchStartTime { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class MatchAttributeValidator : AbstractValidator<MatchAttribute>, IFILValidator
    {
        public MatchAttributeValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}