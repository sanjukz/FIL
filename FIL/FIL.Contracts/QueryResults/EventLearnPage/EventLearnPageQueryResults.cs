using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.EventLearnPage
{
    public class EventLearnPageQueryResults
    {
        public Event Event { get; set; }
        public EventType EventType { get; set; }
        public EventCategory EventCategory { get; set; }
        public EventDetail EventDetail { get; set; }
        public Venue Venue { get; set; }
        public City City { get; set; }
        public State State { get; set; }
        public Models.Country Country { get; set; }
        public IEnumerable<Rating> Rating { get; set; }
        public IEnumerable<Models.UserProfile> User { get; set; }
        public List<EventGalleryImage> EventGalleryImage { get; set; }
    }
}