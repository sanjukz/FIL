using FIL.Contracts.Enums;
using FIL.Contracts.Models;

namespace FIL.Contracts.QueryResults.EventHomePage
{
    public class EventHomePageQueryResult
    {
        public Event Event { get; set; }
        public EventType EventType { get; set; }
        public EventCategory EventCategory { get; set; }
        public EventDetail EventDetail { get; set; }
        public Venue Venue { get; set; }
        public City City { get; set; }
        public State State { get; set; }
        public Models.Country Country { get; set; }
    }
}