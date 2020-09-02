using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.ResetPassword
{
    public class ResetPasswordFormDataViewModel
    {
        public Guid? AltId { get; set; }
        public string Password { get; set; }
        public bool? IsRequestedUserDetails { get; set; }
    }
}
