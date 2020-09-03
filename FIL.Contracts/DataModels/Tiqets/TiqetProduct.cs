using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Tiqets
{
    public class TiqetProduct : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string ProductId { get; set; }
        public string Tittle { get; set; }
        public string SaleStatus { get; set; }
        public string Inclusions { get; set; }
        public string Language { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string ProductSlug { get; set; }
        public decimal Price { get; set; }
        public string SaleStatuReason { get; set; }
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public string Summary { get; set; }
        public string TagLine { get; set; }
        public string PromoLabel { get; set; }
        public string RatingAverage { get; set; }
        public string GeoLocationLatitude { get; set; }
        public string GeoLocationLongitude { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TiqetProductValidator : AbstractValidator<TiqetProduct>, IFILValidator
    {
        public TiqetProductValidator()
        {
            RuleFor(s => s.ProductId).NotEmpty().WithMessage("Product Id is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}