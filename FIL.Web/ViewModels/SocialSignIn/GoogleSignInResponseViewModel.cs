using FIL.Web.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.SocialSignIn
{
    public class GoogleSignInResponseViewModel
    {
        public bool Success { get; set; }
        public SessionViewModel Session { get; set; }        
        public bool? IsEmailRequired { get; set; }        
    }
}
