using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class AlgoliaCitiesExport : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string ObjectId { get; set; }
        public string CityName { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public long CityId { get; set; }
        public long CountryId { get; set; }
        public long StateId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsIndexed { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class AlgoliaCitiesExportValidator : AbstractValidator<AlgoliaCitiesExport>, IKzValidator
    {
        public AlgoliaCitiesExportValidator()
        {
            RuleFor(s => s.ObjectId).NotEmpty().WithMessage("ObjectId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}