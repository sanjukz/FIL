using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CorporateRequest : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public string SponsorName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int ZipCode { get; set; }
        public string PickupRepresentativeFirstName { get; set; }
        public string PickupRepresentativeLastName { get; set; }
        public string PickupRepresentativeEmail { get; set; }
        public string PickupRepresentativePhoneCode { get; set; }
        public string PickupRepresentativePhoneNumber { get; set; }
        public long? SponsorId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool? RequestOrderType { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string Company { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CorporateRequestValidator : AbstractValidator<CorporateRequest>, IKzValidator
    {
        public CorporateRequestValidator()
        {
            RuleFor(s => s.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(s => s.SponsorName).NotEmpty().WithMessage("SponsorName is required");
            RuleFor(s => s.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(s => s.LastName).NotEmpty().WithMessage("LastName is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}