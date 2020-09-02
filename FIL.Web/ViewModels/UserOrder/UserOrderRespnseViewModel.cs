using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Models;

namespace FIL.Web.Feel.ViewModels.UserOrder
{
    public class UserOrderRespnseViewModel
    {
        public IEnumerable<FIL.Contracts.Models.Transaction> Transaction { get; set; }
        public IEnumerable<TransactionDetail> transactionDetail { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<TransactionPaymentDetail> TransactionPaymentDetail { get; set; }
        public IEnumerable<Event> Event { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<CurrencyTypeContainer> CurrencyType { get; set; }
        public IEnumerable<EventCategory> EventCategories { get; set; }
        public IEnumerable<EventCategoryMapping> EventCategoryMappings { get; set; }
        public DateTime CurrentDateTime { get; set; }
    }
}
