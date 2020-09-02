using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class AdvancedSearchContainer
    {
        public SearchResultEvent SearchResult { get; set; }
        public IEnumerable<City> City { get; set; }
        public IEnumerable<State> State { get; set; }
        public IEnumerable<Country> Country { get; set; }
        public Event Event { get; set; }
        public string EventType { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public IEnumerable<Venue> Venue { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
    }
}