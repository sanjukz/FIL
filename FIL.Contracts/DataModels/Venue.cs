using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class Venue : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public int CityId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool? HasImages { get; set; }
        public string Prefix { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class VenueValidator : AbstractValidator<Venue>, IFILValidator
    {
        public VenueValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("name is required");
            RuleFor(s => s.AddressLineOne).NotEmpty().WithMessage("AddressLineOne is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}