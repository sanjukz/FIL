using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Commands.Users
{
    public class RegisterUserCommand : BaseCommand
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
        public Site? SiteId { get; set; }
        public string InviteCode { get; set; }
        public string BirthDate { get; set; }
        public Guid? ResidentOf { get; set; }
        public Guid? CitizenOf { get; set; }
        public bool? IsMailOpt { get; set; }
        public string Ip { get; set; }
        public string ReferralId { get; set; }
    }
}