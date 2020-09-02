using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CitySightSeeingRoute : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long EventDetailId { get; set; }
        public string RouteId { get; set; }
        public string RouteName { get; set; }
        public string RouteColor { get; set; }
        public string RouteDuration { get; set; }
        public string RouteType { get; set; }
        public string RouteStartTime { get; set; }
        public string RouteEndTime { get; set; }
        public string RouteFrequency { get; set; }
        public string RouteLiveLanguages { get; set; }
        public string RouteAudioLanguages { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingRoutesValidator : AbstractValidator<CitySightSeeingRoute>, IKzValidator
    {
        public CitySightSeeingRoutesValidator()
        {
            RuleFor(s => s.EventDetailId).NotEmpty().WithMessage("EventDetailId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}