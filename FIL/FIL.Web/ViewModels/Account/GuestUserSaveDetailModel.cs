using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Account
{
    public class GuestUserSaveDetailModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
    }
}
