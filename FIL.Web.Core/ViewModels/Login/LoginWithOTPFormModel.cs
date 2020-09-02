using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Web.Core.ViewModels.Login
{
    public class LoginWithOTPFormModel
    {
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Channels? ChannelId { get; set; }
        public SignUpMethods? SignUpMethodId { get; set; }
        public Site? SiteId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReferralId { get; set; }
    }
}
