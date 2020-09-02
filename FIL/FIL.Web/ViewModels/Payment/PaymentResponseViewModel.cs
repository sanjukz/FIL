using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Payment
{
    public class PaymentResponseViewModel
    {
        public Guid TransactionAltId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public PaymentGatewayError? PaymentGatewayError { get; set; }
    }
}
