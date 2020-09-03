using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class EventSiteContentMapping : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public Site SiteId { get; set; }
        public string SiteTitle { get; set; }
        public string SiteLogo { get; set; }
        public string BannerText { get; set; }
        public SiteLevel SiteLevel { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class EventSiteContentMappingValidator : AbstractValidator<EventSiteContentMapping>, IFILValidator
    {
        public EventSiteContentMappingValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}