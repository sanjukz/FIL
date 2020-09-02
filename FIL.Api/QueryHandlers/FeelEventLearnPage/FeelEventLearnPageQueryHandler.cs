using AutoMapper;
using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Api.Repositories.Tiqets;
using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.FeelEventLearnPage;
using FIL.Contracts.QueryResults.EventLearnPage;
using FIL.Contracts.QueryResults.FeelEventLearnPage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.FeelEventLearnPage
{
    public class FeelEventLearnPageQueryHandler : IQueryHandler<FeelEventLearnPageQuery, FeelEventLearnPageQueryResult>
    {
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventAmenityRepository _eventAmenityRepository;
        private readonly IEventLearnPageProvider _eventLearnPageProvider;
        private readonly IClientPointOfContactRepository _clientPointOfContactRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventLearnMoreAttributeRepository _eventLearnMoreAttributeRepository;
        private readonly IAmenityRepository _amenityRepository;
        private readonly ICalendarProvider _calendarProvider;
        private readonly ICitySightSeeingRouteRepository _citySightSeeingRouteRepository;
        private readonly ICitySightSeeingRouteDetailRepository _citySightSeeingRouteDetailRepository;
        private readonly ITiqetProductCheckoutDetailRepository _tiqetProductCheckoutDetailRepository;
        private readonly ITiqetEventDetailMappingRepository _tiqetEventDetailMappingRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IEventHostMappingRepository _eventHostMappingRepository;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IDayTimeMappingsRepository _dayTimeMappingsRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly ILocalTimeZoneConvertProvider _localTimeZoneConvertProvider;
        private readonly ITicketAlertEventMappingRepository _ticketAlertEventMappingRepository;

        public FeelEventLearnPageQueryHandler(IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventLearnPageProvider eventLearnPageProvider,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IUserRepository userRepository,
            IRatingRepository ratingRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IEventAmenityRepository eventAmenityRepository,
            IClientPointOfContactRepository clientPointOfContactRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IEventLearnMoreAttributeRepository eventLearnMoreAttributeRepository,
                  IEventCategoryMappingRepository eventCategoryMappingRepository,
            ICalendarProvider calendarProvider,
            IEventCategoryRepository eventCategoryRepository,
            ILocalTimeZoneConvertProvider localTimeZoneConvertProvider,
            ITicketAlertEventMappingRepository ticketAlertEventMappingRepository,
            IAmenityRepository amenityRepository, ICitySightSeeingRouteRepository citySightSeeingRouteRepository, ICitySightSeeingRouteDetailRepository citySightSeeingRouteDetailRepository, ITiqetProductCheckoutDetailRepository tiqetProductCheckoutDetailRepository, ITiqetEventDetailMappingRepository tiqetEventDetailMappingRepository, IEventHostMappingRepository eventHostMappingRepository, IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository, IDayTimeMappingsRepository dayTimeMappingsRepository, IEventAttributeRepository eventAttributeRepository)
        {
            _eventLearnPageProvider = eventLearnPageProvider;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventAmenityRepository = eventAmenityRepository;
            _clientPointOfContactRepository = clientPointOfContactRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _eventLearnMoreAttributeRepository = eventLearnMoreAttributeRepository;
            _amenityRepository = amenityRepository;
            _calendarProvider = calendarProvider;
            _citySightSeeingRouteRepository = citySightSeeingRouteRepository;
            _citySightSeeingRouteDetailRepository = citySightSeeingRouteDetailRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _tiqetProductCheckoutDetailRepository = tiqetProductCheckoutDetailRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _tiqetEventDetailMappingRepository = tiqetEventDetailMappingRepository;
            _eventHostMappingRepository = eventHostMappingRepository;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepository;
            _dayTimeMappingsRepository = dayTimeMappingsRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _ticketAlertEventMappingRepository = ticketAlertEventMappingRepository;
            _localTimeZoneConvertProvider = localTimeZoneConvertProvider;
        }

        public FeelEventLearnPageQueryResult Handle(FeelEventLearnPageQuery query)
        {
            EventLearnPageQueryResults eventLearnMorePage = new EventLearnPageQueryResults();

            if (!String.IsNullOrWhiteSpace(query.Slug))
            {
                eventLearnMorePage = _eventLearnPageProvider.GetBySlug(query.Slug);
            }
            else
            {
                eventLearnMorePage = _eventLearnPageProvider.GetByAltId(query.EventAltId);
            }

            if (eventLearnMorePage.Event != null)
            {
                var eventCategoryMappings = _eventCategoryMappingRepository.GetByEventId(eventLearnMorePage.Event.Id).FirstOrDefault();
                if (eventCategoryMappings == null)
                {
                    return new FeelEventLearnPageQueryResult();
                }
                var subCategoryDataModel = _eventCategoryRepository.Get(eventCategoryMappings.EventCategoryId);
                var subCategory = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(subCategoryDataModel);
                var categoryDataModel = _eventCategoryRepository.Get(subCategory.EventCategoryId);
                var category = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(categoryDataModel);

                var clientPointOfContact = _clientPointOfContactRepository.Get(eventLearnMorePage.Event.ClientPointOfContactId);
                var clientPointOfContactModel = AutoMapper.Mapper.Map<Contracts.Models.ClientPointOfContact>(clientPointOfContact);

                var eventTicketDetails = _eventTicketDetailRepository.GetByEventDetailId(eventLearnMorePage.EventDetail.Id).Where(s => s.IsEnabled == true);
                var EventTicketDetailModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventTicketDetail>>(eventTicketDetails);

                var eventTicketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetails.Select(s => s.TicketCategoryId).Distinct());
                var eventTicketCategoriesModel = AutoMapper.Mapper.Map<List<Contracts.Models.TicketCategory>>(eventTicketCategories);

                var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetails.Select(s => s.Id).Distinct());
                var eventTicketAttributesModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventTicketAttribute>>(eventTicketAttributes);
                if (eventTicketAttributesModel.Count() == 0)
                {
                    return new FeelEventLearnPageQueryResult { };
                }
                var currencyMapping = _currencyTypeRepository.GetByCurrencyId(eventTicketAttributesModel.First().CurrencyId);

                var ratingDataModel = _ratingRepository.GetByEventId(eventLearnMorePage.Event.Id);
                var ratingModel = AutoMapper.Mapper.Map<List<Contracts.Models.Rating>>(ratingDataModel);

                var userdataModel = _userRepository.GetByUserIds(ratingModel.Select(s => s.UserId).Distinct());
                var userModel = AutoMapper.Mapper.Map<List<Contracts.Models.UserProfile>>(userdataModel);

                var eventAmenities = _eventAmenityRepository.GetByEventId(eventLearnMorePage.Event.Id);
                var eventLearnMoreAttributes = _eventLearnMoreAttributeRepository.GetByEventId(eventLearnMorePage.Event.Id);
                var learnMoreAttributes = AutoMapper.Mapper.Map<List<Contracts.Models.EventLearnMoreAttribute>>(eventLearnMoreAttributes);
                var ticketAlertEventMapping = _ticketAlertEventMappingRepository.GetByEventId(eventCategoryMappings.EventId).FirstOrDefault();
                List<string> EventAmenitiesList = new List<string>();
                foreach (var item in eventAmenities)
                {
                    var amenities = _amenityRepository.Get(item.AmenityId);
                    EventAmenitiesList.Add((amenities.Amenity).ToString());
                }
                var data = _calendarProvider.GetCalendarData(eventLearnMorePage.Event.Id);
                //For Hoho Routes
                var citySightSeeingRoute = _citySightSeeingRouteRepository.GetByEventDetailId(eventLearnMorePage.EventDetail.Id);
                var citySightSeeingRouteDetails = _citySightSeeingRouteDetailRepository.GetByCitySightSeeingRouteIds(citySightSeeingRoute.Select(s => s.Id));

                //for Tiqets Places
                Contracts.Models.Tiqets.TiqetProductCheckoutDetail
                                  tiqetCheckoutDetailsModel = new Contracts.Models.Tiqets.TiqetProductCheckoutDetail();
                if (eventLearnMorePage.Event.EventSourceId == EventSource.Tiqets)
                {
                    var tiqetEventDetailMapping = _tiqetEventDetailMappingRepository.GetByEventDetailId(eventLearnMorePage.EventDetail.Id);
                    var tiqetCheckoutDetails = _tiqetProductCheckoutDetailRepository.GetByProductId(tiqetEventDetailMapping.ProductId);
                    tiqetCheckoutDetailsModel = Mapper.Map<Contracts.Models.Tiqets.TiqetProductCheckoutDetail>(tiqetCheckoutDetails);
                }

                //For Live Online Events
                List<EventHostMapping> eventHostMappings = new List<EventHostMapping>();
                DateTime formattedDateTime = new DateTime();
                string eventDateTimeZome = string.Empty;
                if (eventLearnMorePage.Event.MasterEventTypeId == MasterEventType.Online)
                {
                    var eventHostMappingModel = _eventHostMappingRepository.GetAllByEventId(eventLearnMorePage.Event.Id);
                    eventHostMappings = AutoMapper.Mapper.Map<List<Contracts.Models.EventHostMapping>>(eventHostMappingModel);
                    var placeWeekOpenDays = _placeWeekOpenDaysRepository.GetByEventId(eventLearnMorePage.Event.Id).FirstOrDefault();
                    var dayTimeMapping = _dayTimeMappingsRepository.GetAllByPlaceWeekOpenDay(placeWeekOpenDays.Id).FirstOrDefault();
                    var eventAttribute = _eventAttributeRepository.GetByEventDetailId(eventLearnMorePage.EventDetail.Id);

                    //Getting Datetime for online Event
                    var eventDate = _localTimeZoneConvertProvider.ConvertToLocal(eventLearnMorePage.EventDetail.StartDateTime, eventAttribute.TimeZone);
                    var dateTimeString = eventDate.ToString("yyyy/MM/dd") + " " + dayTimeMapping.StartTime + ":00.000";
                    formattedDateTime = DateTime.Parse(dateTimeString);

                    eventDateTimeZome = eventAttribute.TimeZoneAbbreviation;
                }
                return new FeelEventLearnPageQueryResult
                {
                    EventType = (EventType)eventLearnMorePage.EventType,
                    EventCategory = eventLearnMorePage.EventCategory,
                    Event = eventLearnMorePage.Event,
                    EventDetail = eventLearnMorePage.EventDetail,
                    Venue = eventLearnMorePage.Venue,
                    City = eventLearnMorePage.City,
                    State = eventLearnMorePage.State,
                    Country = eventLearnMorePage.Country,
                    EventTicketAttribute = eventTicketAttributesModel,
                    EventTicketDetail = EventTicketDetailModel,
                    CurrencyType = Mapper.Map<CurrencyType>(currencyMapping),
                    Rating = ratingModel,
                    TicketCategory = eventTicketCategoriesModel,
                    User = userModel,
                    EventAmenitiesList = EventAmenitiesList,
                    ClientPointOfContact = clientPointOfContactModel,
                    EventGalleryImage = eventLearnMorePage.EventGalleryImage,
                    EventLearnMoreAttributes = learnMoreAttributes,
                    RegularTimeModel = data.RegularTimeModel,
                    SeasonTimeModel = data.SeasonTimeModel,
                    SpecialDayModel = data.SpecialDayModel,
                    Category = category,
                    SubCategory = subCategory,
                    CitySightSeeingRoutes = Mapper.Map<IEnumerable<FIL.Contracts.Models.CitySightSeeing.CitySightSeeingRoute>>(citySightSeeingRoute),
                    CitySightSeeingRouteDetails = Mapper.Map<IEnumerable<FIL.Contracts.Models.CitySightSeeing.CitySightSeeingRouteDetail>>(citySightSeeingRouteDetails),
                    TiqetsCheckoutDetails = tiqetCheckoutDetailsModel,
                    EventHostMappings = eventHostMappings,
                    OnlineStreamStartTime = formattedDateTime,
                    OnlineEventTimeZone = eventDateTimeZome,
                    TicketAlertEventMapping = ticketAlertEventMapping
                };
            }
            return new FeelEventLearnPageQueryResult();
        }
    }
}