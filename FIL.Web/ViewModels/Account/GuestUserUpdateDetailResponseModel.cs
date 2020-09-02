using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Account
{
    public class GuestUserUpdateDetailResponseModel
    {
        public bool Success { get; set; }
        public FIL.Contracts.Models.UserProfile UserProfile { get; set; }
    }
}
