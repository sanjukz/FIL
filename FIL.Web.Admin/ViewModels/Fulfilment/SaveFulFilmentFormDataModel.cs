using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Fulfilment
{
    public class SaveFulFilmentFormDataModel
    {
        public int pickupOTP { get; set; }
        public long transactionDetailId { get; set; }
        public string ticketNumber { get; set; }
    }
}
