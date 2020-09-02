using FIL.Contracts.Models;
using FIL.Contracts.Models.ASI;
using FIL.Contracts.Models.CitySightSeeing;
using FIL.Contracts.Models.Tiqets;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TicketCategories
{
    public class TicketCategoryQueryResult
    {
        public Event Event { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<Models.Venue> Venue { get; set; }
        public IEnumerable<City> City { get; set; }
        public IEnumerable<Team> Team { get; set; }
        public IEnumerable<MatchAttribute> MatchAttribute { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<TicketFeeDetail> TicketFeeDetail { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<RASVTicketTypeMapping> RASVTicketTypeMappings { get; set; }
        public IEnumerable<EventDeliveryTypeDetail> EventDeliveryTypeDetails { get; set; }
        public IEnumerable<PlaceHolidayDate> PlaceHolidayDates { get; set; }
        public IEnumerable<PlaceWeekOff> PlaceWeekOffs { get; set; }
        public IEnumerable<PlaceCustomerDocumentTypeMapping> PlaceCustomerDocumentTypeMappings { get; set; }
        public IEnumerable<CustomerDocumentType> CustomerDocumentTypes { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string EventCategoryName { get; set; }
        public int EventCategory { get; set; }
        public List<FIL.Contracts.DataModels.TicketCategoryType> TicketCategoryTypes { get; set; }
        public List<FIL.Contracts.DataModels.TicketCategorySubType> TicketCategorySubTypes { get; set; }
        public FIL.Contracts.DataModels.EventCategoryMapping EventCategoryMappings { get; set; }
        public List<FIL.Contracts.Models.EventTicketDetailTicketCategoryTypeMapping> EventTicketDetailTicketCategoryTypeMappings { get; set; }
        public RegularViewModel RegularTimeModel;
        public List<SeasonViewModel> SeasonTimeModel;
        public List<SpecialDayViewModel> SpecialDayModel;
        public EventVenueMapping EventVenueMappings { get; set; }
        public IEnumerable<EventVenueMappingTime> EventVenueMappingTimes { get; set; }
        public IEnumerable<EventTimeSlotMapping> EventTimeSlotMappings { get; set; }
        public IEnumerable<PlaceWeekOpenDays> PlaceWeekOpenDays { get; set; }
        public IEnumerable<Days> Days { get; set; }
        public IEnumerable<FIL.Contracts.DataModels.CountryRegionalOrganisationMapping> CountryRegionalOrganisationMappings { get; set; }
        public IEnumerable<string> RegionalOrganisation { get; set; }
        public IEnumerable<FIL.Contracts.Models.Country> Countries { get; set; }
        public Models.Tiqets.TiqetProductCheckoutDetail TiqetsCheckoutDetails { get; set; }
        public List<ValidWithVariantModel> ValidWithVariantModel { get; set; }
        public EventCategory Category { get; set; }
        public EventCategory SubCategory { get; set; }
        public CitySightSeeingTicketDetail CitySightSeeingTicketDetail { get; set; }
        public List<FIL.Contracts.Models.POne.POneImageEventDetailMapping> POneImageEventDetailMapping { get; set; }
        public List<EventHostMapping> EventHosts { get; set; }
        public List<FIL.Contracts.DataModels.EventAttribute> EventAttributes { get; set; }
        public List<FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel> eventRecurranceScheduleModels { get; set; }
        public Item ASIMonument { get; set; }
        public List<FIL.Contracts.DataModels.EventHostMapping> EventHostMapping { get; set; }
        public string FormattedDateString { get; set; }
    }
}