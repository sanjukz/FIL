using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.OrderConformation
{
    public class GetOrderConfirmationDataViewModel
    {
        public Guid transactionId { get; set; }
        public bool confirmationFromMyOrders { get; set; }
    }
}
