using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Payment
{
    public class PaymentFormResponseViewModel
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string Method { get; set; }
        public string Action { get; set; }
        public Dictionary<string, string> FormFields { get; set; }
        public Guid TransactionAltId { get; set; }
    }
}
