using FIL.Contracts.Enums;

namespace FIL.Contracts.Commands.Users
{
    public class RasvRegisterUserCommand : BaseCommand
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public int RolesId { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public Channels? ChannelId { get; set; }
        public bool? OptedForMailer { get; set; }
        public int? OptOutStatusId { get; set; }
        public string SocialLoginId { get; set; }
        public SignUpMethods SignUpMethodId { get; set; }
        public Site? SiteId { get; set; }
        public string ReferralId { get; set; }
        public string Ip { get; set; }
    }
}