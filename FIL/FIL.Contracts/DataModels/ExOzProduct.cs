using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ExOzProduct : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string UrlSegment { get; set; }
        public string Summary { get; set; }
        public long OperatorId { get; set; }
        public long EventDetailId { get; set; }
        public int VenueId { get; set; }
        public string CanonicalRegionUrlSegment { get; set; }
        public bool? BookingRequired { get; set; }
        public string HandlerKey { get; set; }
        public string Title { get; set; }
        public string Timestamp { get; set; }
        public string OperatorName { get; set; }
        public string Description { get; set; }
        public string MoreInfo { get; set; }
        public string Tips { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string HelpCode { get; set; }
        public string Timezone { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ExOzProductValidator : AbstractValidator<ExOzProduct>, IKzValidator
    {
        public ExOzProductValidator()
        {
            RuleFor(s => s.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(s => s.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(s => s.UrlSegment).NotEmpty().WithMessage("UrlSegment is required");
            RuleFor(s => s.OperatorId).NotEmpty().WithMessage("OperatorId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}