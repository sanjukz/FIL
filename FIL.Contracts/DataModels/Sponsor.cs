using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class Sponsor : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string SponsorName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
        public SponsorType SponsorTypeId { get; set; }
        public string CityId { get; set; }
        public string StateId { get; set; }
        public string CountryId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class SponsorsValidator : AbstractValidator<Sponsor>, IKzValidator
    {
        public SponsorsValidator()
        {
            RuleFor(s => s.SponsorName).NotEmpty().WithMessage("SponsorName is required");
            RuleFor(s => s.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(s => s.LastName).NotEmpty().WithMessage("LastName is required");
            RuleFor(s => s.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(s => s.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}