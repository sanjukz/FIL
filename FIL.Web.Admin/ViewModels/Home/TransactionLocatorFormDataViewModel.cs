using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Home
{
    public class TransactionLocatorFormDataViewModel
    {
        public long ConfirmationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string UserMobileNo { get; set; }
        public string BarcodeNumber { get; set; }
    }
}
