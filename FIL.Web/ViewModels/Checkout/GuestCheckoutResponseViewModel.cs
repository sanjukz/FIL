using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Checkout
{
    public class GuestCheckoutResponseViewModel
    {
        public bool Success { get; set; }
        public long TransactionId { get; set; }
        public string EventName { get; set; }
        public string TicketCategoryName { get; set; }
        public bool IsTransactionLimitExceed { get; set; }
        public bool IsTicketCategorySoldOut { get; set; }
    }
}
