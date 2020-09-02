using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Account
{
    public class ChangePasswordFormDataViewModel
    {
        public Guid AltId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
