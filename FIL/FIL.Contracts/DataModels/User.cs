using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class User : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public int RolesId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? EmailVerified { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneConfirmed { get; set; }

        //public OptOutStatus OptOutStatusId { get; set; }
        public string SocialLoginId { get; set; }

        public SignUpMethods? SignUpMethodId { get; set; }
        public Channels? ChannelId { get; set; }
        public bool IsActivated { get; set; }
        public long? ReferralId { get; set; }
        public DateTime? LockOutEndDateUtc { get; set; }
        public bool LockOutEnabled { get; set; }
        public short AccessFailedCount { get; set; }
        public int LoginCount { get; set; }
        public int? SecurityStamp { get; set; }
        public bool IsEnabled { get; set; }
        public bool? IsRASVMailOPT { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool ProfilePic { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public int? IPDetailId { get; set; }
        public int? CountryId { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class UserValidator : AbstractValidator<User>, IKzValidator
    {
        public UserValidator()
        {
            RuleFor(s => s.Email).NotEmpty().WithMessage("UserName is required");
            RuleFor(s => s.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(s => s.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(s => s.LastName).NotEmpty().WithMessage("LastName is required");
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}