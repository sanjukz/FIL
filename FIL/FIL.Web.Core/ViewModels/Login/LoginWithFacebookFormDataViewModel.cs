using FIL.Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace FIL.Web.Core.ViewModels.Login
{
    public class LoginWithFacebookFormDataViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string SocialLoginId { get; set; }
        public Channels? ChannelId { get; set; }
        public SignUpMethods? SignUpMethodId { get; set; }
        public Site? SiteId { get; set; }
    }
}
