using AutoMapper;
using FIL.Api.Providers;
using FIL.Api.Providers.EventManagement;
using FIL.Api.Repositories;
using FIL.Api.Repositories.Tiqets;
using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Contracts.Models.Tiqets;
using FIL.Contracts.Queries.TicketCategory;
using FIL.Contracts.QueryResults.TicketCategories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FIL.Api.QueryHandlers.TicketCategory
{
    public class TicketCategoryQueryHandler : IQueryHandler<TicketCategoryQuery, TicketCategoryQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IRASVTicketTypeMappingRepository _rasvTicketTypeMappingRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetail;
        private readonly ITicketFeeDetailRepository _ticketFeeDetail;
        private readonly ITeamRepository _teamRepository;
        private readonly IMatchAttributeRepository _matchAttributeRepository;
        private readonly IPlaceWeekOffRepository _placeWeekOffRepository;
        private readonly IPlaceHolidayDatesRepository _placeHolidayDatesRepository;
        private readonly IPlaceCustomerDocumentTypeMappingRepository _placeCustomerDocumentTypeMappingRepository;
        private readonly ICustomerDocumentTypeRepository _customerDocumentTypeRepository;
        private readonly ITicketCategoryTypesRepository _ticketCategoryTypesRepository;
        private readonly ITicketCategorySubTypesRepository _ticketCategorySubTypesRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IEventTicketDetailTicketCategoryTypeMappingRepository _eventTicketDetailTicketCategoryTypeMappingRepository;
        private readonly ICalendarProvider _calendarProvider;
        private readonly IEventVenueMappingRepository _eventVenueMappingRepository;
        private readonly IEventVenueMappingTimeRepository _eventVenueMappingTimeRepository;
        private readonly IEventTimeSlotMappingRepository _eventTimeSlotMappingRepository;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IDaysRepository _daysRepository;
        private readonly ICountryRegionalOrganisationMappingRepository _countryRegionalOrganisationMappingRepository;
        private readonly ITiqetProductCheckoutDetailRepository _tiqetProductCheckoutDetailRepository;
        private readonly ITiqetEventDetailMappingRepository _tiqetEventDetailMappingRepository;
        private readonly ITiqetVariantDetailRepository _tiqetVariantDetailRepository;
        private readonly ITiqetEventTicketDetailMappingRepository _tiqetEventTicketDetailMappingRepository;
        private readonly ICitySightSeeingEventDetailMappingRepository _citySightSeeingEventDetailMappingRepository;
        private readonly ICitySightSeeingTicketRepository _citySightSeeingTicketRepository;
        private readonly ICitySightSeeingTicketDetailRepository _citySightSeeingTicketDetailRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IPOneEventTicketDetailRepository _pOneEventTicketDetailRepository;
        private readonly IPOneEventDetailMappingRepository _pOneEventDetailMappingRepository;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IPOneTicketCategoryRepository _pOneTicketCategoryRepository;
        private readonly IEventHostMappingRepository _eventHostMappingRepository;
        private readonly ILocalTimeZoneConvertProvider _localTimeZoneConvertProvider;
        private readonly IGetScheduleDetailProvider _getScheduleDetailProvider;
        private readonly IASIMonumentEventTableMappingRepository _aSIMonumentEventTableMappingRepository;
        private readonly IASIMonumentRepository _aSIMonumentRepository;

        public TicketCategoryQueryHandler(IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IPlaceWeekOffRepository placeWeekOffRepository,
            IEventTicketDetailTicketCategoryTypeMappingRepository eventTicketDetailTicketCategoryTypeMappingRepository,
            IVenueRepository venueRepository,
            ICityRepository cityRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IEventDeliveryTypeDetailRepository eventDeliveryTypeDetail,
            IRASVTicketTypeMappingRepository rasvTicketTypeMappingRepository,
            ITicketFeeDetailRepository ticketFeeDetail,
            ITeamRepository teamRepository,
            IEventCategoryMappingRepository eventCategoryMappingRepository,
            IPlaceHolidayDatesRepository placeHolidayDatesRepository,
            IPlaceCustomerDocumentTypeMappingRepository placeCustomerDocumentTypeMappingRepository,
            ICustomerDocumentTypeRepository customerDocumentTypeRepository,
             ITicketCategoryTypesRepository ticketCategoryTypesRepository,
            ITicketCategorySubTypesRepository ticketCategorySubTypesRepository,
            ICalendarProvider calendarProvider,
            IMatchAttributeRepository matchAttributeRepository,
            IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository,
            IEventVenueMappingRepository eventVenueMappingRepository,
            IEventTimeSlotMappingRepository eventTimeSlotMappingRepository,
            IDaysRepository daysRepository,
            ICountryRegionalOrganisationMappingRepository countryRegionalOrganisationMappingRepository,
            ICountryRepository countryRepository,
            IEventCategoryRepository eventCategoryRepository,
            FIL.Logging.ILogger logger,
            IEventVenueMappingTimeRepository eventVenueMappingTimeRepository, ITiqetProductCheckoutDetailRepository tiqetProductCheckoutDetailRepository, ITiqetEventDetailMappingRepository tiqetEventDetailMappingRepository, ITiqetVariantDetailRepository tiqetVariantDetailRepository, ITiqetEventTicketDetailMappingRepository tiqetEventTicketDetailMappingRepository,
            ICitySightSeeingEventDetailMappingRepository citySightSeeingEventDetailMappingRepository, ICitySightSeeingTicketRepository citySightSeeingTicketRepository, ICitySightSeeingTicketDetailRepository citySightSeeingTicketDetailRepository,
            IPOneEventTicketDetailRepository pOneEventTicketDetailRepository,
            IPOneEventDetailMappingRepository pOneEventDetailMappingRepository,
            IEventAttributeRepository eventAttributeRepository,
            IPOneTicketCategoryRepository pOneTicketCategoryRepository,
            ILocalTimeZoneConvertProvider localTimeZoneConvertProvider,
            IGetScheduleDetailProvider getScheduleDetailProvider,
            IEventHostMappingRepository eventHostMappingRepository,
            IASIMonumentEventTableMappingRepository aSIMonumentEventTableMappingRepository,
            IASIMonumentRepository aSIMonumentRepository)

        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _teamRepository = teamRepository;
            _matchAttributeRepository = matchAttributeRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _rasvTicketTypeMappingRepository = rasvTicketTypeMappingRepository;
            _eventDeliveryTypeDetail = eventDeliveryTypeDetail;
            _ticketFeeDetail = ticketFeeDetail;
            _placeWeekOffRepository = placeWeekOffRepository;
            _placeHolidayDatesRepository = placeHolidayDatesRepository;
            _customerDocumentTypeRepository = customerDocumentTypeRepository;
            _placeCustomerDocumentTypeMappingRepository = placeCustomerDocumentTypeMappingRepository;
            _ticketCategoryTypesRepository = ticketCategoryTypesRepository;
            _ticketCategorySubTypesRepository = ticketCategorySubTypesRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _eventTicketDetailTicketCategoryTypeMappingRepository = eventTicketDetailTicketCategoryTypeMappingRepository;
            _calendarProvider = calendarProvider;
            _eventVenueMappingRepository = eventVenueMappingRepository;
            _eventVenueMappingTimeRepository = eventVenueMappingTimeRepository;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepository;
            _eventTimeSlotMappingRepository = eventTimeSlotMappingRepository;
            _daysRepository = daysRepository;
            _countryRegionalOrganisationMappingRepository = countryRegionalOrganisationMappingRepository;
            _countryRepository = countryRepository;
            _tiqetProductCheckoutDetailRepository = tiqetProductCheckoutDetailRepository;
            _tiqetEventDetailMappingRepository = tiqetEventDetailMappingRepository;
            _logger = logger;
            _tiqetVariantDetailRepository = tiqetVariantDetailRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _tiqetEventTicketDetailMappingRepository = tiqetEventTicketDetailMappingRepository;
            _citySightSeeingEventDetailMappingRepository = citySightSeeingEventDetailMappingRepository;
            _citySightSeeingTicketRepository = citySightSeeingTicketRepository;
            _citySightSeeingTicketDetailRepository = citySightSeeingTicketDetailRepository;
            _pOneEventTicketDetailRepository = pOneEventTicketDetailRepository;
            _pOneEventDetailMappingRepository = pOneEventDetailMappingRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _pOneTicketCategoryRepository = pOneTicketCategoryRepository;
            _eventHostMappingRepository = eventHostMappingRepository;
            _localTimeZoneConvertProvider = localTimeZoneConvertProvider;
            _getScheduleDetailProvider = getScheduleDetailProvider;
            _aSIMonumentEventTableMappingRepository = aSIMonumentEventTableMappingRepository;
            _aSIMonumentRepository = aSIMonumentRepository;
        }

        public TicketCategoryQueryResult Handle(TicketCategoryQuery query)
        {
            try
            {
                var eventDataModel = _eventRepository.GetByAltId(query.EventAltId);
                var eventModel = AutoMapper.Mapper.Map<Contracts.Models.Event>(eventDataModel);
                if (eventModel != null)
                {
                    var ASIMonumentMapping = _aSIMonumentEventTableMappingRepository.GetByEventId(eventModel.Id);
                    var ASIMonument = new Contracts.Models.ASI.Item();
                    if (ASIMonumentMapping != null)
                    {
                        ASIMonument = Mapper.Map<Contracts.Models.ASI.Item>(_aSIMonumentRepository.Get(ASIMonumentMapping.ASIMonumentId));
                    }
                    var eventCategoryMappings = _eventCategoryMappingRepository.GetByEventId(eventModel.Id).FirstOrDefault();
                    var eventDetailModelDataModel = _eventDetailRepository.GetSubEventByEventId(eventModel.Id);
                    var eventDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventDetail>>(eventDetailModelDataModel);
                    List<FIL.Contracts.Models.ASI.EventTimeSlotMapping> eventTimeSlotMappings = new List<Contracts.Models.ASI.EventTimeSlotMapping>();
                    List<EventVenueMappingTime> eventVenueMappingTimeModel = new List<EventVenueMappingTime>();
                    FIL.Contracts.Models.EventCategory subCategory = new FIL.Contracts.Models.EventCategory();
                    FIL.Contracts.Models.EventCategory category = new FIL.Contracts.Models.EventCategory();
                    try
                    {
                        var eventTimeSlot = _eventTimeSlotMappingRepository.GetByEventId(eventModel.Id);
                        eventTimeSlotMappings = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.ASI.EventTimeSlotMapping>>(eventTimeSlot).ToList();
                    }
                    catch (Exception e)
                    {
                    }
                    if (eventCategoryMappings != null)
                    {
                        var subCategoryDataModel = _eventCategoryRepository.Get(eventCategoryMappings.EventCategoryId);
                        if (subCategory != null)
                        {
                            subCategory = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(subCategoryDataModel);
                            var categoryDataModel = _eventCategoryRepository.Get(subCategory.EventCategoryId);
                            category = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(categoryDataModel);
                        }
                    }
                    var ticketCategoryType = _ticketCategoryTypesRepository.GetAll();
                    var ticketCategorySubType = _ticketCategorySubTypesRepository.GetAll();

                    var placeHolidyDatesDataModel = _placeHolidayDatesRepository.GetAllByEventId(eventDataModel.Id);
                    var placeHolidyDatesModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.PlaceHolidayDate>>(placeHolidyDatesDataModel);

                    var placeWeekOffDataModel = _placeWeekOffRepository.GetAllByEventId(eventDataModel.Id);
                    var placeWeekOffModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.PlaceWeekOff>>(placeWeekOffDataModel);

                    var placeDocumentDataModel = _placeCustomerDocumentTypeMappingRepository.GetAllByEventId(eventDataModel.Id);
                    var placeDocumentModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.PlaceCustomerDocumentTypeMapping>>(placeDocumentDataModel);

                    var placeOpenDays = _placeWeekOpenDaysRepository.GetByEventId(eventDataModel.Id);
                    var placeOpenDaysModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.PlaceWeekOpenDays>>(placeOpenDays);

                    var CustomerDocumentTypeDataModel = _customerDocumentTypeRepository.GetAll();
                    var CustomerDocumentTypeModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.CustomerDocumentType>>(CustomerDocumentTypeDataModel);

                    if (eventDetailModelDataModel != null && eventDetailModelDataModel.Any())
                    {
                        var eventDeliveryTypeDetailDataModel = _eventDeliveryTypeDetail.GetByEventDetailId(eventDetailModelDataModel.ElementAt(0).Id);
                        var eventDeliveryTypeDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventDeliveryTypeDetail>>(eventDeliveryTypeDetailDataModel);

                        var RASVTicketTypeMappingsDataModel = _rasvTicketTypeMappingRepository.GetByEventDetailIds(eventDetailModelDataModel.Select(ed => ed.Id)).Where(sed => sed.IsEnabled == true);
                        var RASVTicketTypeMappingsModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.RASVTicketTypeMapping>>(RASVTicketTypeMappingsDataModel);

                        var venueDetailDataModel = _venueRepository.GetByVenueIds(eventDetailModel.Select(s => s.VenueId));
                        var venueDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.Venue>>(venueDetailDataModel);

                        //Multiple Venue option for MOVE AROUND category.
                        var eventVenueMappingDataModel = _eventVenueMappingRepository.GetOneByEventId(eventModel.Id);
                        var eventVenueMappingModel = Mapper.Map<EventVenueMapping>(eventVenueMappingDataModel);
                        if (eventVenueMappingModel != null)
                        {
                            var eventVenueMappingTimeDataModel = _eventVenueMappingTimeRepository.GetAllByEventVenueMappingId(eventVenueMappingModel.Id);
                            eventVenueMappingTimeModel = Mapper.Map<IEnumerable<EventVenueMappingTime>>(eventVenueMappingTimeDataModel).ToList();
                        }

                        var CityList = venueDetailDataModel.Select(s => s.CityId);
                        var cityDetailDataModel = _cityRepository.GetByCityIds(CityList);
                        var cityModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.City>>(cityDetailDataModel);

                        var eventTicketDetailList = eventDetailModelDataModel.Select(s => s.Id);
                        var eventTicketDetailDataModel = _eventTicketDetailRepository.GetByEventDetailIds(eventTicketDetailList);
                        var eventTicketDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketDetail>>(eventTicketDetailDataModel);

                        // For Getting Pone venue Images
                        var pOneImageEventDetailMapping = new List<Contracts.Models.POne.POneImageEventDetailMapping>();
                        if (eventModel.EventSourceId == EventSource.POne)
                        {
                            var pOneEventDetailMappings = _pOneEventDetailMappingRepository.GetByEventDetailIds(eventTicketDetailModel.Select(s => s.EventDetailId).Distinct().ToList());
                            var allTicketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetailModel.Select(s => s.TicketCategoryId).Distinct().ToList());
                            var allPoneETD = _pOneEventTicketDetailRepository.GetByManyPOneEventDetail(pOneEventDetailMappings.Select(s => s.POneEventDetailId).Distinct().ToList());
                            foreach (var item in eventTicketDetailModel)
                            {
                                var pOneEventDetailMappingData = pOneEventDetailMappings.Where(s => s.ZoongaEventDetailId == item.EventDetailId).FirstOrDefault();
                                var tc = allTicketCategories.Where(s => s.Id == item.TicketCategoryId).FirstOrDefault();
                                var p1tc = _pOneTicketCategoryRepository.GetByName(tc.Name);
                                if (pOneEventDetailMappingData != null && p1tc != null)
                                {
                                    var p1etd = allPoneETD.Where(s => (s.POneEventDetailId == pOneEventDetailMappingData.POneEventDetailId && s.POneTicketCategoryId == p1tc.POneId)).FirstOrDefault();
                                    if (p1etd != null)
                                    {
                                        pOneImageEventDetailMapping.Add(new Contracts.Models.POne.POneImageEventDetailMapping
                                        {
                                            EventTicketDetailId = item.Id,
                                            ImageUrl = p1etd.ImageUrl
                                        });
                                    }
                                }
                            }
                        }

                        var eventTicketDetailTicketCategoryMappings = _eventTicketDetailTicketCategoryTypeMappingRepository.GetByEventTicketDetails(eventTicketDetailModel.Select(s => s.Id).ToList());
                        var eventTicketDetailTicketCategoryMappingsModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketDetailTicketCategoryTypeMapping>>(eventTicketDetailTicketCategoryMappings).ToList();

                        var matchAttribute = _matchAttributeRepository.GetByEventDetailIds(eventDetailModelDataModel.Select(ed => ed.Id).Distinct());
                        var team = _teamRepository.GetAll();
                        var data = _calendarProvider.GetCalendarData(eventModel.Id);
                        if (eventTicketDetailModel != null)
                        {
                            var ticketCategoryIdList = eventTicketDetailModel.Select(s => s.TicketCategoryId);
                            var ticketCategoryDataModel = _ticketCategoryRepository.GetByEventDetailIds(ticketCategoryIdList);
                            var ticketCategoryModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketCategory>>(ticketCategoryDataModel);

                            var eventTicketDetailIdList = eventTicketDetailModel.Select(s => s.Id);
                            var eventTicketDetailIdDataModel = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetailIdList);
                            var eventTicketAttributeModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(eventTicketDetailIdDataModel);

                            var eventTicketAttributeIdList = eventTicketAttributeModel.Select(s => s.Id);
                            var ticketFeeDetailIdDataModel = _ticketFeeDetail.GetByEventTicketAttributeIds(eventTicketAttributeIdList);
                            var ticketFeeDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketFeeDetail>>(ticketFeeDetailIdDataModel);

                            var currencyList = eventTicketAttributeModel.Select(s => s.CurrencyId).Distinct().FirstOrDefault();
                            var currencyModel = AutoMapper.Mapper.Map<Contracts.Models.CurrencyType>(_currencyTypeRepository.Get(currencyList));

                            var days = _daysRepository.GetAll();
                            var daysModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.Days>>(days);

                            var CountryRegionalOrganisation = _countryRegionalOrganisationMappingRepository.GetAll();

                            var eventAttribute = _eventAttributeRepository.GetByEventDetailIds(eventDetailModel.Select(s => (long)s.Id).ToList());

                            var Country = _countryRepository.GetAll();
                            var Countrymodel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.Country>>(Country);
                            List<string> reginalOrganisation = new List<string>();
                            reginalOrganisation.AddRange(Enum.GetNames(typeof(FIL.Contracts.Enums.RegionalOrganisation)));

                            // For Tiqet's gettting Checkout Details
                            TiqetProductCheckoutDetail
                                tiqetCheckoutDetailsModel = new TiqetProductCheckoutDetail();
                            List<ValidWithVariantModel> validWithVariantList = new List<ValidWithVariantModel>();
                            if (eventModel.EventSourceId == EventSource.Tiqets)
                            {
                                var tiqetEventDetailMapping = _tiqetEventDetailMappingRepository.GetByEventDetailId(eventDetailModel.ElementAt(0).Id);
                                var tiqetCheckoutDetails = _tiqetProductCheckoutDetailRepository.GetByProductId(tiqetEventDetailMapping.ProductId);
                                tiqetCheckoutDetailsModel = Mapper.Map<TiqetProductCheckoutDetail>(tiqetCheckoutDetails);
                                // for checking valid with tiqetVariant details
                                var tiqetVariantDetails = _tiqetVariantDetailRepository.GetAllByProductId(tiqetCheckoutDetailsModel.ProductId);

                                List<long> eventTicketDetailIds = new List<long>();
                                foreach (var currentVariantDetail in tiqetVariantDetails)
                                {
                                    if (currentVariantDetail.ValidWithVariantIds != null && currentVariantDetail.ValidWithVariantIds != "")
                                    {
                                        ValidWithVariantModel validWithVariantModel = new ValidWithVariantModel();
                                        var validWithVariantIds = currentVariantDetail.ValidWithVariantIds.Split(",");
                                        foreach (var currentValidvariantId in validWithVariantIds)
                                        {
                                            var currentValidVariantDetail = tiqetVariantDetails.Where(s => s.VariantId == Convert.ToInt64(currentValidvariantId)).FirstOrDefault();
                                            if (currentValidVariantDetail != null)
                                            {
                                                var eventTicketDetailMapping = _tiqetEventTicketDetailMappingRepository.GetByTiqetVariantId(currentValidVariantDetail.Id);
                                                eventTicketDetailIds.Add(eventTicketDetailMapping.EventTicketDetailId);
                                            }
                                        }
                                        var currentEventTicketDetailMapping = _tiqetEventTicketDetailMappingRepository.GetByTiqetVariantId(currentVariantDetail.Id);
                                        validWithVariantModel.EventTicketDetailId = currentEventTicketDetailMapping.EventTicketDetailId;
                                        validWithVariantModel.ValidWithEventTicketDetailId = eventTicketDetailIds;
                                        validWithVariantList.Add(validWithVariantModel);
                                    }
                                }
                            }
                            // Check for Hoho Places if any
                            FIL.Contracts.DataModels.CitySightSeeingTicketDetail citySightSeeingTicketDetail = new FIL.Contracts.DataModels.CitySightSeeingTicketDetail();
                            var citySightSeeingEventDetailMapping = _citySightSeeingEventDetailMappingRepository.GetByEventDetailId(eventDetailModel.ElementAt(0).Id);
                            if (citySightSeeingEventDetailMapping != null)
                            {
                                var citySightSeeingTickets = _citySightSeeingTicketRepository.Get(citySightSeeingEventDetailMapping.CitySightSeeingTicketId);
                                citySightSeeingTicketDetail = _citySightSeeingTicketDetailRepository.GetByTicketId(citySightSeeingTickets.TicketId);
                            }
                            //Get Host Details for FIL Online Events
                            var eventHostMappingList = new List<FIL.Contracts.DataModels.EventHostMapping>();
                            var formattedDateString = "";
                            if (eventDataModel.MasterEventTypeId == MasterEventType.Online)
                            {
                                eventHostMappingList = _eventHostMappingRepository.GetAllByEventId(eventDataModel.Id).ToList();
                                var formattedDateTime = _localTimeZoneConvertProvider.ConvertToLocal(eventDetailModel.FirstOrDefault().StartDateTime, eventAttribute.FirstOrDefault().TimeZone);
                                formattedDateString = formattedDateTime.DayOfWeek + ", " + formattedDateTime.ToString(@"MMM dd, yyyy, hh:mm tt", new CultureInfo("en-US"));
                            }
                            //Get Recurrance Schedule
                            List<FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel> recurranceScheduleModels = new List<Contracts.Models.CreateEventV1.EventRecurranceScheduleModel>();
                            if (eventDetailModel.FirstOrDefault().EventFrequencyType == EventFrequencyType.Recurring)
                            {
                                recurranceScheduleModels = _getScheduleDetailProvider.GetScheduleDetails(eventDataModel.Id, DateTime.UtcNow, DateTime.UtcNow, false, true).Where(s => s.EndDateTime > DateTime.UtcNow).ToList();
                            }
                            try
                            {
                                return new TicketCategoryQueryResult
                                {
                                    Event = eventModel,
                                    EventDetail = eventDetailModel,
                                    EventTicketAttribute = eventTicketAttributeModel,
                                    TicketFeeDetail = ticketFeeDetailModel,
                                    Venue = venueDetailModel,
                                    City = cityModel,
                                    EventTicketDetail = eventTicketDetailModel,
                                    TicketCategory = ticketCategoryModel,
                                    CurrencyType = currencyModel,
                                    RASVTicketTypeMappings = RASVTicketTypeMappingsModel,
                                    EventDeliveryTypeDetails = eventDeliveryTypeDetailModel,
                                    EventCategory = eventModel.EventCategoryId,
                                    MatchAttribute = Mapper.Map<IEnumerable<MatchAttribute>>(matchAttribute),
                                    Team = Mapper.Map<IEnumerable<Team>>(team),
                                    PlaceCustomerDocumentTypeMappings = placeDocumentModel,
                                    PlaceHolidayDates = placeHolidyDatesModel,
                                    PlaceWeekOffs = placeWeekOffModel,
                                    CustomerDocumentTypes = CustomerDocumentTypeModel,
                                    TicketCategorySubTypes = ticketCategorySubType.ToList(),
                                    TicketCategoryTypes = ticketCategoryType.ToList(),
                                    EventTicketDetailTicketCategoryTypeMappings = eventTicketDetailTicketCategoryMappingsModel,
                                    EventCategoryMappings = eventCategoryMappings,
                                    RegularTimeModel = data.RegularTimeModel,
                                    SeasonTimeModel = data.SeasonTimeModel,
                                    SpecialDayModel = data.SpecialDayModel,
                                    EventVenueMappings = eventVenueMappingModel,
                                    EventVenueMappingTimes = eventVenueMappingTimeModel,
                                    EventTimeSlotMappings = eventTimeSlotMappings,
                                    PlaceWeekOpenDays = placeOpenDaysModel,
                                    Days = daysModel,
                                    CountryRegionalOrganisationMappings = CountryRegionalOrganisation.ToList(),
                                    Countries = Countrymodel,
                                    Category = category,
                                    SubCategory = subCategory,
                                    RegionalOrganisation = reginalOrganisation,
                                    TiqetsCheckoutDetails = tiqetCheckoutDetailsModel,
                                    ValidWithVariantModel = validWithVariantList,
                                    CitySightSeeingTicketDetail = Mapper.Map<FIL.Contracts.Models.CitySightSeeing.CitySightSeeingTicketDetail>(citySightSeeingTicketDetail),
                                    POneImageEventDetailMapping = pOneImageEventDetailMapping,
                                    EventHostMapping = eventHostMappingList,
                                    EventAttributes = eventAttribute.ToList(),
                                    eventRecurranceScheduleModels = recurranceScheduleModels,
                                    ASIMonument = ASIMonument,
                                    FormattedDateString = formattedDateString
                                };
                            }
                            catch (Exception e)
                            {
                                return new TicketCategoryQueryResult
                                {
                                };
                            }
                        }
                        else
                        {
                            return new TicketCategoryQueryResult
                            {
                                Event = eventModel,
                                EventDetail = eventDetailModel,
                                EventTicketAttribute = null,
                                TicketFeeDetail = null,
                                Venue = venueDetailModel,
                                City = cityModel,
                                EventTicketDetail = eventTicketDetailModel,
                                TicketCategory = null,
                                CurrencyType = null,
                                RASVTicketTypeMappings = RASVTicketTypeMappingsModel,
                                EventDeliveryTypeDetails = eventDeliveryTypeDetailModel,
                                EventCategory = eventModel.EventCategoryId,
                                MatchAttribute = null,
                                Team = null,
                                CitySightSeeingTicketDetail = null
                            };
                        }
                    }
                    else
                    {
                        return new TicketCategoryQueryResult { };
                    }
                }
                else
                {
                    return new TicketCategoryQueryResult { };
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return new TicketCategoryQueryResult { };
            }
        }
    }
}