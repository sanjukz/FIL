using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TournamentLayoutSection : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public int TournamentLayoutId { get; set; }
        public int TournamentLayoutSectionId { get; set; }
        public int Capacity { get; set; }
        public int EntryGateId { get; set; }
        public VenueLayoutArea VenueLayoutAreaId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TournamentLayoutSectionValidator : AbstractValidator<TournamentLayoutSection>, IKzValidator
    {
        public TournamentLayoutSectionValidator()
        {
            RuleFor(s => s.SectionName).NotEmpty().WithMessage("SectionName is required");
            RuleFor(s => s.Capacity).NotEmpty().WithMessage("Capacity is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}