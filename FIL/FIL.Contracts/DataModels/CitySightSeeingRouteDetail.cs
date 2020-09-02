using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CitySightSeeingRouteDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long CitySightSeeingRouteId { get; set; }
        public string RouteLocationName { get; set; }
        public string RouteLocationId { get; set; }
        public string RouteLocationDescription { get; set; }
        public string RouteLocationLatitude { get; set; }
        public string RouteLocationLongitude { get; set; }
        public bool RouteLocationStopover { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingRouteDetailValidator : AbstractValidator<CitySightSeeingRouteDetail>, IKzValidator
    {
        public CitySightSeeingRouteDetailValidator()
        {
            RuleFor(s => s.CitySightSeeingRouteId).NotEmpty().WithMessage("CitySight Seeing RouteId is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}