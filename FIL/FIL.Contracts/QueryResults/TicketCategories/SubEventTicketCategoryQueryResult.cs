using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TicketCategories
{
    public class SubEventTicketCategoryQueryResult
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