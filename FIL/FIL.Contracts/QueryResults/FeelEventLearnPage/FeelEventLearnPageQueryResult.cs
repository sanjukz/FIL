using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Contracts.Models.CitySightSeeing;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.FeelEventLearnPage
{
    public class FeelEventLearnPageQueryResult
    {
        public Event Event { get; set; }
        public EventType EventType { get; set; }
        public EventCategory EventCategory { get; set; }
        public ClientPointOfContact ClientPointOfContact { get; set; }
        public EventDetail EventDetail { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public Venue Venue { get; set; }
        public City City { get; set; }
        public State State { get; set; }
        public Models.Country Country { get; set; }
        public List<string> Amenities { get; set; }
        public IEnumerable<Rating> Rating { get; set; }
        public IEnumerable<Models.UserProfile> User { get; set; }
        public List<string> EventAmenitiesList { get; set; }
        public List<EventGalleryImage> EventGalleryImage { get; set; }
        public IEnumerable<EventLearnMoreAttribute> EventLearnMoreAttributes { get; set; }
        public RegularViewModel RegularTimeModel;
        public List<SeasonViewModel> SeasonTimeModel;
        public List<SpecialDayViewModel> SpecialDayModel;
        public IEnumerable<CitySightSeeingRoute> CitySightSeeingRoutes { get; set; }
        public IEnumerable<CitySightSeeingRouteDetail> CitySightSeeingRouteDetails { get; set; }
        public IEnumerable<EventHostMapping> EventHostMappings { get; set; }
        public Models.Tiqets.TiqetProductCheckoutDetail TiqetsCheckoutDetails { get; set; }
        public FIL.Contracts.DataModels.TicketAlertEventMapping TicketAlertEventMapping { get; set; }
        public EventCategory Category { get; set; }
        public EventCategory SubCategory { get; set; }
        public DateTime OnlineStreamStartTime { get; set; }
        public string OnlineEventTimeZone { get; set; }
    }
}