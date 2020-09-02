using FIL.Web.Feel.ViewModels.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Account
{
    public class ChangePasswordResponseViewModel : ResponseViewModel
    {
        public bool WrongPassword { get; set; }
        public FIL.Contracts.Models.UserProfile Profile { get; set; }

    }
}
