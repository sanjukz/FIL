using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Models;


namespace FIL.Web.Feel.ViewModels.TicketCategories
{
    public class SubEventTicketCategoryResponseViewModel
    {
        public EventDetail EventDetail { get; set; }
        public Venue Venue { get; set; }
        public City City { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public CurrencyType CurrencyType { get; set; }
    }
}
