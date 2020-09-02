using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Commands.Transaction
{
    public class GuestUserDetail
    {
        public int Age { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public int Gender { get; set; }
        public string IdentityNumber { get; set; }
        public int IdentityType { get; set; }
        public string LastName { get; set; }
        public Guid PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public Channels? ChannelId { get; set; }
        public SignUpMethods? SignUpMethodId { get; set; }
        public bool? IsMailOpt { get; set; }
        public Guid Country { get; set; }
    }
}