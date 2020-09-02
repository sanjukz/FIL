using FIL.Web.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.SocialSignIn
{
    public class FacebookSignInResponseViewModel
    {
        public bool Success { get; set; }
        public bool IsEmailReqd { get; set; }
        public SessionViewModel Session { get; set; }
    }
}
