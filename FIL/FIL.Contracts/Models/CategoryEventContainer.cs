using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class CategoryEventContainer
    {
        public CategoryEvent CategoryEvent { get; set; }
        public IEnumerable<City> City { get; set; }
        public IEnumerable<State> State { get; set; }
        public IEnumerable<Country> Country { get; set; }
        public Event Event { get; set; }
        public string EventType { get; set; }
        public string LocalStartDateTime { get; set; }
        public string TimeZoneAbbrivation { get; set; }
        public string EventCategory { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public IEnumerable<Venue> Venue { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<Rating> Rating { get; set; }
        public string ParentCategory { get; set; }
        public List<string> EventCategories { get; set; }
        public string Duration { get; set; }
        public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
        public FIL.Contracts.DataModels.LiveEventDetail LiveEventDetail { get; set; }
    }
}