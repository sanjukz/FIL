using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Models;

namespace FIL.Web.Feel.ViewModels.TicketCategories
{
    public class MoveAroundTicketCategoryResponseViewModel
    {
        public Event Event { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<Venue> Venue { get; set; }
        public IEnumerable<City> City { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<TicketFeeDetail> TicketFeeDetail { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<EventDeliveryTypeDetail> EventDeliveryTypeDetails { get; set; }
        public IEnumerable<PlaceHolidayDate> PlaceHolidayDates { get; set; }
        public IEnumerable<PlaceWeekOff> PlaceWeekOffs { get; set; }
        public IEnumerable<PlaceCustomerDocumentTypeMapping> PlaceCustomerDocumentTypeMappings { get; set; }
        public IEnumerable<CustomerDocumentType> CustomerDocumentTypes { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string EventCategoryName { get; set; }
        public string EventCategory { get; set; }
        public List<FIL.Contracts.DataModels.TicketCategoryType> TicketCategoryTypes { get; set; }
        public List<FIL.Contracts.DataModels.TicketCategorySubType> TicketCategorySubTypes { get; set; }
        public FIL.Contracts.DataModels.EventCategoryMapping EventCategoryMappings { get; set; }
        public List<FIL.Contracts.Models.EventTicketDetailTicketCategoryTypeMapping> EventTicketDetailTicketCategoryTypeMappings { get; set; }
        public RegularViewModel RegularTimeModel;
        public List<SeasonViewModel> SeasonTimeModel;
        public List<SpecialDayViewModel> SpecialDayModel;
        public IEnumerable<EventVenueMapping> EventVenueMappings { get; set; }
        public IEnumerable<EventVenueMappingTime> EventVenueMappingTimes { get; set; }
        public List<string> DeliveryOptions;
    }
}
