using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class AlgoliaExport : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string ObjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Url { get; set; }
        public string PlaceImageUrl { get; set; }
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

    public class AlgoliaExportValidator : AbstractValidator<AlgoliaExport>, IKzValidator
    {
        public AlgoliaExportValidator()
        {
            RuleFor(s => s.ObjectId).NotEmpty().WithMessage("ObjectId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}