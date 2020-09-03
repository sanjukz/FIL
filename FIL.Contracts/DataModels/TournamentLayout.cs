using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TournamentLayout : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string LayoutName { get; set; }
        public long EventId { get; set; }
        public int MasterVenueLayoutId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TournamentLayoutValidator : AbstractValidator<TournamentLayout>, IFILValidator
    {
        public TournamentLayoutValidator()
        {
            RuleFor(s => s.LayoutName).NotEmpty().WithMessage("LayoutName is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}