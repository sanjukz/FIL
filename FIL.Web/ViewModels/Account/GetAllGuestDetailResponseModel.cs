using System;
using FIL.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Account
{
    public class GetAllGuestDetailResponseModel
    {
        public List<GuestUserAdditionalDetail> GuestUserDetails { get; set; }
    }
}
