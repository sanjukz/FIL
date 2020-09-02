using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.EventLearnPage
{
    public class WebEventLearnPageQueryResults
    {
        public Event Event { get; set; }
        public EventType? EventType { get; set; }
        public EventCategory EventCategory { get; set; }
        public List<EventDetail> EventDetail { get; set; }
        public List<Venue> Venue { get; set; }
        public List<City> City { get; set; }
        public List<State> State { get; set; }
        public List<Models.Country> Country { get; set; }
        public IEnumerable<Rating> Rating { get; set; }
        public IEnumerable<Models.UserProfile> User { get; set; }
        public List<EventGalleryImage> EventGalleryImage { get; set; }
    }
}