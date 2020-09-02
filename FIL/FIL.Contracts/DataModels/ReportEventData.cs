using System.Collections.Generic;

namespace FIL.Contracts.DataModels
{
    public class ReportEventData
    {
        public IEnumerable<Event> Event { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<EventAttribute> EventAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<TicketFeeDetail> TicketFeeDetail { get; set; }
        public IEnumerable<Venue> Venue { get; set; }
        public IEnumerable<City> City { get; set; }
        public IEnumerable<State> State { get; set; }
        public IEnumerable<Country> Country { get; set; }
    }
}