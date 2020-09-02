using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzOperator : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long OperatorId { get; set; }
        public string Name { get; set; }
        public string PublicName { get; set; }
        public string UrlSegment { get; set; }
        public string CanonicalRegionUrlSegment { get; set; }
        public long RegionId { get; set; }
        public long? EventId { get; set; }
        public int? VenueId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Tips { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Rating { get; set; }
        public int? Quantity { get; set; }
        public string Timestamp { get; set; }
        public string GeoLocationName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzOperatorValidator : AbstractValidator<ExOzOperator>, IKzValidator
    {
        public ExOzOperatorValidator()
        {
            RuleFor(s => s.OperatorId).NotEmpty().WithMessage("OperatorId is required");
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(s => s.UrlSegment).NotEmpty().WithMessage("UrlSegment is required");
            RuleFor(s => s.CanonicalRegionUrlSegment).NotEmpty().WithMessage("CanonicalRegionUrlSegment is required");
            RuleFor(s => s.RegionId).NotEmpty().WithMessage("RegionId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}