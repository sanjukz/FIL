using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Account
{
    public class ChangePrimaryPhoneFormDataViewModel
    {
        public Guid AltId { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
