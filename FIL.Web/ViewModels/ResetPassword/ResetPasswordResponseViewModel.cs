using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.ResetPassword
{
    public class ResetPasswordResponseViewModel
    {
        public bool Success { get; set; }
        public UserProfile User { get; set; }
    }
}
