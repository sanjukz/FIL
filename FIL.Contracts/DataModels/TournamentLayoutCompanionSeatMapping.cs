using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TournamentLayoutCompanionSeatMapping : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long WheelChairSeatId { get; set; }
        public long CompanionSeatId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TournamentLayoutCompanionSeatMappingValidator : AbstractValidator<TournamentLayoutCompanionSeatMapping>, IKzValidator
    {
        public TournamentLayoutCompanionSeatMappingValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}