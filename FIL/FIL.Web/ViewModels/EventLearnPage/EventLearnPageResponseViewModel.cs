using FIL.Contracts.Models;
using System.Collections.Generic;
using FIL.Web.Feel.ViewModels.Category;
using FIL.Contracts.Models.CitySightSeeing;
using System;

namespace FIL.Web.Feel.ViewModels.EventLearnPage
{
    public class EventLearnPageResponseViewModel
    {
        public Event Event { get; set; }
        public string EventType { get; set; }
        public string EventCategory { get; set; }
        public EventDetail EventDetail { get; set; }
        public ClientPointOfContact ClientPointOfContact { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public Venue Venue { get; set; }
        public City City { get; set; }
        public State State { get; set; }
        public FIL.Contracts.Models.Country Country { get; set; }
        public IEnumerable<Rating> Rating { get; set; }
        public IEnumerable<UserProfile> User { get; set; }
        public List<string> EventAmenitiesList { get; set; }
        public List<CategoryEventContainer> Categories { get; set; }
        public List<EventGalleryImage> EventGalleryImage { get; set; }
        public string EventCategoryName { get; set; }
        public IEnumerable<EventLearnMoreAttribute> EventLearnMoreAttributes { get; set; }
        public List<UserImageMap> UserImageMap { get; set; }
        public RegularViewModel RegularTimeModel;
        public List<SeasonViewModel> SeasonTimeModel;
        public List<SpecialDayViewModel> SpecialDayModel;
        public IEnumerable<CitySightSeeingRoute> CitySightSeeingRoutes { get; set; }
        public IEnumerable<CitySightSeeingRouteDetail> CitySightSeeingRouteDetails { get; set; }
        public IEnumerable<EventHostMapping> EventHostMappings { get; set; }
        public Contracts.Models.Tiqets.TiqetProductCheckoutDetail TiqetsCheckoutDetails { get; set; }
        public Contracts.DataModels.TicketAlertEventMapping TicketAlertEventMapping { get; set; }
        public EventCategory Category { get; set; }
        public EventCategory SubCategory { get; set; }
        public DateTime OnlineStreamStartTime { get; set; }
        public string OnlineEventTimeZone { get; set; }

    }
}
