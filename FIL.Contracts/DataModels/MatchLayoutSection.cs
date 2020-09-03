using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class MatchLayoutSection : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public int TournamentLayoutSectionId { get; set; }
        public int MatchLayoutId { get; set; }
        public int Capacity { get; set; }
        public int EntryGateId { get; set; }
        public VenueLayoutArea VenueLayoutAreaId { get; set; }
        public int MatchLayoutSectionId { get; set; }
        public long TicketCategoryId { get; set; }
        public bool? IsPrintBigX { get; set; }
        public bool? IsPrintSeatNumber { get; set; }
        public string StaircaseNumber { get; set; }
        public string RampNumber { get; set; }
        public string ScanGateName { get; set; }
        public bool IsEnabled { get; set; }
        public long? EventTicketDetailId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class MatchLayoutSectionValidator : AbstractValidator<MatchLayoutSection>, IFILValidator
    {
        public MatchLayoutSectionValidator()
        {
            RuleFor(s => s.SectionName).NotEmpty().WithMessage("SectionName is required");
            RuleFor(s => s.Capacity).NotEmpty().WithMessage("Capacity is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}