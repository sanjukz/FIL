using FIL.Contracts.Commands.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Checkout
{
    public class GuestTransactionFormDataViewModel
    {
        public GuestCheckoutFormDataViewModel UserDetail { get; set; }
        public List<EventTicketAttribute> EventTicketAttributeList { get; set; }
    }
}
