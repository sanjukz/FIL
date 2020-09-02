using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Models;
using FIL.Contracts.Models.CitySightSeeing;

namespace FIL.Web.Feel.ViewModels.TicketCategories
{
    public class TicketCategoryResponseViewModel
    {
        public Event Event { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<PlaceHolidayDate> PlaceHolidayDates { get; set; }
        public IEnumerable<PlaceWeekOff> PlaceWeekOffs { get; set; }
        public IEnumerable<PlaceCustomerDocumentTypeMapping> PlaceCustomerDocumentTypeMappings { get; set; }
        public IEnumerable<CustomerDocumentType> CustomerDocumentTypes { get; set; }
        public IEnumerable<Venue> Venue { get; set; }
        public IEnumerable<City> City { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventDeliveryTypeDetail> EventDeliveryTypeDetails { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string EventCategory { get; set; }
        public string EventCategoryName { get; set; }
        public List<FIL.Contracts.DataModels.TicketCategoryType> TicketCategoryTypes { get; set; }
        public List<FIL.Contracts.DataModels.TicketCategorySubType> TicketCategorySubTypes { get; set; }
        public List<FIL.Contracts.Models.EventTicketDetailTicketCategoryTypeMapping> EventTicketDetailTicketCategoryTypeMappings { get; set; }
        public RegularViewModel RegularTimeModel;
        public List<SeasonViewModel> SeasonTimeModel;
        public List<SpecialDayViewModel> SpecialDayModel;
        public List<string> DeliveryOptions;
        public EventVenueMapping EventVenueMappings { get; set; }
        public IEnumerable<EventVenueMappingTime> EventVenueMappingTimes { get; set; }
        public Contracts.Models.Tiqets.TiqetProductCheckoutDetail TiqetsCheckoutDetails { get; set; }
        public List<FIL.Contracts.Models.Tiqets.ValidWithVariantModel> ValidWithVariantModel { get; set; }
        public EventCategory Category { get; set; }
        public EventCategory SubCategory { get; set; }
        public CitySightSeeingTicketDetail CitySightSeeingTicketDetail { get; set; }
        public List<FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel> eventRecurranceScheduleModels { get; set; }
        public List<FIL.Contracts.DataModels.EventHostMapping> EventHostMapping { get; set; }
        public List<FIL.Contracts.DataModels.EventAttribute> EventAttributes { get; set; }
        public string FormattedDateString { get; set; }
    }
}
