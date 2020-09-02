using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Payment
{
    public class PaymentResponseFormDataViewModel
    {
        public long TransactionId { get; set; }
        public string QueryString { get; set; }
        public int PaymentOption { get; set; }
    }
}
