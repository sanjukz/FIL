using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class UserAddressDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Guid AltId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int? CityId { get; set; }
        public int Zipcode { get; set; }
        public AddressTypes? AddressTypeId { get; set; }
        public bool? IsDefault { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class UserAddressDetailValidator : AbstractValidator<UserAddressDetail>, IKzValidator
    {
        public UserAddressDetailValidator()
        {
            RuleFor(s => s.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(s => s.LastName).NotEmpty().WithMessage("LastName is required");
            RuleFor(s => s.PhoneCode).NotEmpty().WithMessage("PhoneCode is required");
            RuleFor(s => s.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required");
            RuleFor(s => s.AddressLine1).NotEmpty().WithMessage("AddressLine1 is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}