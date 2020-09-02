using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.Users
{
    public class RegisterUserWithOTPCommand : ICommandWithResult<RegisterUserWithOTPCommandCommandResult>
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public Channels? ChannelId { get; set; }
        public SignUpMethods? SignUpMethodId { get; set; }
        public string Ip { get; set; }
        public string ReferralId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class RegisterUserWithOTPCommandCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public bool? EmailAlreadyRegistered { get; set; }
        public Contracts.DataModels.User User { get; set; }
    }
}