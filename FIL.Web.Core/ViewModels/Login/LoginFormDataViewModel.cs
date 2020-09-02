using FIL.Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace FIL.Web.Core.ViewModels.Login
{
    public class LoginFormDataViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public Channels? ChannelId { get; set; }
        public SignUpMethods? SignUpMethodId { get; set; }
        public Site? SiteId { get; set; }

        public bool RememberLogin { get; set; }
    }
}
