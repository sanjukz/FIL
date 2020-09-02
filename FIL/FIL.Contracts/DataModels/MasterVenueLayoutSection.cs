using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class MasterVenueLayoutSection : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string SectionName { get; set; }
        public int MasterVenueLayoutId { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public int Capacity { get; set; }
        public int EntryGateId { get; set; }
        public int VenueLayoutAreaId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }

        public bool? IsSeatExists { get; set; }
    }

    public class MasterVenueLayoutSectionValidator : AbstractValidator<MasterVenueLayoutSection>, IKzValidator
    {
        public MasterVenueLayoutSectionValidator()
        {
            RuleFor(s => s.SectionName).NotEmpty().WithMessage("SectionName is required");
            RuleFor(s => s.Capacity).NotEmpty().WithMessage("Capacity is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}