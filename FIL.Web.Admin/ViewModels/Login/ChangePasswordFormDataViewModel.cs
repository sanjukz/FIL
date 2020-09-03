using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Account
{
    public class ChangePasswordFormDataViewModel
    {                
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
