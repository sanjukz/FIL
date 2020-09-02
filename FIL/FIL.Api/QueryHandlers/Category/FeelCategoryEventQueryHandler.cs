using AutoMapper;
using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Category;
using FIL.Contracts.QueryResults.Category;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FIL.Api.QueryHandlers.Category
{
    public class FeelCategoryEventQueryHandler : IQueryHandler<FeelCategoryEventQuery, FeelCategoryEventQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IEventTicketDetailTicketCategoryTypeMappingRepository _eventTicketDetailTicketCategoryTypeMappingRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly ICountryDescriptionRepository _countryDescriptionRepository;
        private readonly ICountryContentMappingRepository _countryContentMappingRepository;
        private readonly ICityDescriptionRepository _cityDescriptionRepository;
        private readonly ILocalTimeZoneConvertProvider _localTimeZoneConvertProvider;
        private readonly ILiveEventDetailRepository _liveEventDetailRepository;

        public FeelCategoryEventQueryHandler(IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository,
            IEventDetailRepository eventDetailRepository,
            IEventAttributeRepository eventAttributeRepository,
            IVenueRepository venueRepository,
            ICountryRepository countryRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventTicketDetailTicketCategoryTypeMappingRepository eventTicketDetailTicketCategoryTypeMappingRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IRatingRepository ratingRepository,
            ICityDescriptionRepository cityDescription,
            IEventSiteIdMappingRepository eventSiteIdMappingRepository,
            ILocalTimeZoneConvertProvider localTimeZoneConvertProvider,
            ILiveEventDetailRepository liveEventDetailRepository,
            IEventCategoryMappingRepository eventCategoryMappingRepository, ICountryDescriptionRepository countryDescriptionRepository, ICountryContentMappingRepository countryContentMappingRepository)
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventTicketDetailTicketCategoryTypeMappingRepository = eventTicketDetailTicketCategoryTypeMappingRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _ratingRepository = ratingRepository;
            _countryDescriptionRepository = countryDescriptionRepository;
            _countryContentMappingRepository = countryContentMappingRepository;
            _cityDescriptionRepository = cityDescription;
            _localTimeZoneConvertProvider = localTimeZoneConvertProvider;
            _liveEventDetailRepository = liveEventDetailRepository;
        }

        public List<long> eventIdProvider(FeelCategoryEventQuery query)
        {
            List<long> indexEventId = new List<long>();
            if (!query.IsCountryLandingPage && !query.IsCityLandingPage)
            {
                int index = query.PageNumber == 1 ? 0 : (query.PageNumber * 8);
                var siteEvents = _eventSiteIdMappingRepository
                    .GetBySiteId(query.SiteId)
                    .OrderBy(o => o.SortOrder).ToList();
                var categoryEventIds = new HashSet<long>();

                if (query.IsAllOnline)
                {
                    categoryEventIds = new HashSet<long>(_eventCategoryMappingRepository.GetByParentEventCategoryId((int)query.EventCategoryId, query.Search, query.IsSearch, query.EventCategories.Select(s => s.Id).ToList(), true)
                       .Select(ce => ce.EventId));
                }
                else if (query.IsAll)
                {
                    categoryEventIds = new HashSet<long>(_eventCategoryMappingRepository.GetByParentEventCategoryId((int)query.EventCategoryId, query.Search, query.IsSearch)
                        .Select(ce => ce.EventId));
                }
                else
                {
                    categoryEventIds = new HashSet<long>(_eventCategoryMappingRepository.GetByEventCategoryId((int)query.EventCategoryId, query.Search, query.IsSearch)
                       .Select(ce => ce.EventId));
                }

                var lookupEventIds = siteEvents.Where(se => categoryEventIds.Contains(se.EventId)).Select(e => e.EventId);

                if (query.IsSimilarListing)
                {
                    var length = 4;
                    if (lookupEventIds.Count() <= 3)
                    {
                        length = lookupEventIds.Count();
                    }
                    for (var i = 0; i < length; i++)
                    {
                        Random r = new Random();
                        int rInt = r.Next(1, lookupEventIds.Count()); //for ints
                        indexEventId.Add(lookupEventIds.ElementAt(rInt - 1));
                    }
                }
                else
                {
                    if (!query.IsAll)
                    {
                        indexEventId = lookupEventIds.ToList();
                    }
                    else
                    {
                        for (int i = index; i <= (index + (query.EventCategoryId == 98 ? 11 : 7)); i++)
                        {
                            if (lookupEventIds.Count() > i)
                            {
                                indexEventId.Add(lookupEventIds.ElementAt(i));
                            }
                        }
                    }
                }
            }
            else if (query.IsCountryLandingPage)
            {
                var events = _eventRepository.GetAllPlaceByCountry(query.CountryName);
                indexEventId = events.Select(s => s.Id).ToList();
            }
            else
            {
                var events = _eventRepository.GetAllPlaceCity(query.CountryName);
                indexEventId = events.Select(s => s.Id).ToList();
            }
            return indexEventId;
        }

        public FeelCategoryEventQueryResult Handle(FeelCategoryEventQuery query)
        {
            try
            {
                query.SiteId = FIL.Contracts.Enums.Site.feelaplaceSite;
                var countryDescription = new FIL.Contracts.DataModels.CountryDescription();
                var cityDescription = new FIL.Contracts.DataModels.CityDescription();
                var countryContents = new List<FIL.Contracts.DataModels.CountryContentMapping>();
                var indexEventId = eventIdProvider(query);
                var siteEvents = _eventSiteIdMappingRepository
                    .GetBySiteId(query.SiteId)
                    .OrderBy(o => o.SortOrder).ToList();
                var categoryEvents = _eventRepository.GetByAllEventIds(indexEventId)
                    .Where(e => e.IsEnabled)
                    .ToList();

                var eventMapping = categoryEvents.ToDictionary(e => e.Id);

                var eventIds = eventMapping.Keys;
                var eventCategoryMappings = _eventCategoryMappingRepository.GetByEventIds(eventIds).Where(ed => ed.IsEnabled == true);
                var allEventDetails = _eventDetailRepository.GetByEventIds(eventIds).Where(ed => ed.IsEnabled == true);
                var allEventAttributes = _eventAttributeRepository.GetByEventDetailIds(allEventDetails.Select(s => s.Id));
                var allEventTicketDetails = _eventTicketDetailRepository.GetAllByEventDetailIds(allEventDetails.Select(s => s.Id));
                var allEventTicketDetailTicketCategoryTypeMapping = _eventTicketDetailTicketCategoryTypeMappingRepository.GetByEventTicketDetails(allEventTicketDetails.Select(s => s.Id).ToList()).Where(s => s.TicketCategoryTypeId == 2);
                allEventTicketDetails = allEventTicketDetails.Where(s => allEventTicketDetailTicketCategoryTypeMapping.All(p => p.EventTicketDetailId != s.Id));
                var allLiveEventDetails = _liveEventDetailRepository.GetAllByEventIds(eventIds.ToList());
                var allEventRatings = _ratingRepository.GetByEventIds(eventIds);
                var venueMapping = _venueRepository.GetByVenueIds(allEventDetails.Select(ed => ed.VenueId).Distinct())
                    .ToDictionary(v => v.Id);
                var eventDetailsMapping = allEventDetails.GroupBy(ed => ed.EventId)
                    .ToDictionary(g => g.Key, g => g.ToList());
                var eventCategoryMapping = eventCategoryMappings.GroupBy(ed => ed.EventId)
                    .ToDictionary(g => g.Key, g => g.ToList());
                var eventRatingMapping = allEventRatings.GroupBy(ed => ed.EventId)
                   .ToDictionary(g => g.Key, g => g.ToList());

                var cityIds = venueMapping.Values.Select(s => s.CityId).Distinct();
                var allCities = _cityRepository.GetByCityIds(cityIds);
                var cityMapping = allCities.ToDictionary(c => c.Id);
                var stateId = cityMapping.Values.Select(c => c.StateId).Distinct();
                var stateMapping = _stateRepository.GetByStateIds(stateId)
                    .ToDictionary(s => s.Id);
                var countryIdList = stateMapping.Values.Select(s => s.CountryId).Distinct();
                var countryMapping = _countryRepository.GetByCountryIds(countryIdList)
                    .ToDictionary(c => c.Id);
                if (query.IsCountryLandingPage)
                {
                    if (countryIdList.ToList().Any())
                    {
                        countryDescription = _countryDescriptionRepository.GetByCountryId(countryIdList.ToList().ElementAt(0));
                        countryContents = _countryContentMappingRepository.GetByCountryId(countryIdList.ToList().ElementAt(0)).ToList();
                    }
                }
                if (query.IsCityLandingPage)
                {
                    if (cityIds.ToList().Any())
                    {
                        var cities = _cityRepository.GetAllByName(allCities.ElementAt(0).Name);
                        foreach (var currentCity in allCities)
                        {
                            cityDescription = _cityDescriptionRepository.GetBycityId(currentCity.Id);
                            if (cityDescription != null)
                            {
                                break;
                            }
                        }
                    }
                }
                var eventsBySortOrder = new List<FIL.Contracts.DataModels.Event>();
                foreach (var item in siteEvents)
                {
                    var events = categoryEvents.FirstOrDefault(w => w.Id == item.EventId);
                    if (events != null)
                    {
                        eventsBySortOrder.Add(AutoMapper.Mapper.Map<FIL.Contracts.DataModels.Event>(events));
                    }
                }
                eventsBySortOrder = eventsBySortOrder.Where(s => !s.IsTokenize).ToList();
                var CategoryEventData = eventsBySortOrder.Select(ce =>
                {
                    try
                    {
                        var eventObj = eventMapping[ce.Id];
                        var EventCategory = eventObj.EventCategoryId.ToString();
                        var parentCategory = "";
                        var eventDetails = eventDetailsMapping[ce.Id];
                        var eventAttribute = allEventAttributes.Where(s => s.EventDetailId == eventDetails.FirstOrDefault().Id).FirstOrDefault();
                        var eventTicketDetails = allEventTicketDetails.Where(s => eventDetails.Exists(p => p.Id == s.EventDetailId));
                        var eventCategoryMap = eventCategoryMapping[ce.Id];
                        if (eventCategoryMap.Any())
                        {
                            var eventCategory = _eventCategoryRepository.Get(eventCategoryMap.FirstOrDefault().EventCategoryId);
                            if (eventCategory != null)
                            {
                                EventCategory = eventCategory.Slug;
                                var parentCategoryModel = _eventCategoryRepository.Get(eventCategory.EventCategoryId);
                                parentCategory = parentCategoryModel.Slug;
                            }
                        }
                        List<FIL.Contracts.DataModels.Rating> eventRatings;
                        if (eventRatingMapping.ContainsKey(ce.Id))
                        {
                            eventRatings = eventRatingMapping[ce.Id];
                        }
                        else
                        {
                            eventRatings = null;
                        }
                        var eventDetailIdList = eventDetails.Select(s => s.Id).Distinct().ToList();
                        var eventTicketDetailIdList = eventTicketDetails.Select(s => s.Id).Distinct().ToList();
                        var venues = eventDetails.Select(s => s.VenueId).Distinct().Select(v => venueMapping[v]);
                        var cities = venues.Select(s => s.CityId).Distinct().Select(c => cityMapping[c]);
                        var states = cities.Select(s => s.StateId).Distinct().Select(s => stateMapping[s]);
                        var countries = states.Select(s => s.CountryId).Distinct().Select(c => countryMapping[c]);
                        var eventTicketAttributeMapping = new List<Contracts.DataModels.EventTicketAttribute>
                    {
                        _eventTicketAttributeRepository.GetMaxPriceByEventTicketDetailId(eventTicketDetailIdList),
                        _eventTicketAttributeRepository.GetMinPriceByEventTicketDetailId(eventTicketDetailIdList)
                    };
                        var currencyMapping = _currencyTypeRepository.GetByCurrencyId(eventTicketAttributeMapping.Where(s => s.IsEnabled == true).ToList().First().CurrencyId);
                        List<string> eventCatMappings = new List<string>();
                        var eventCategoryDataModel = _eventCategoryRepository.GetByIds(eventCategoryMap.Select(s => s.EventCategoryId));
                        foreach (var eventCat in eventCategoryDataModel)
                        {
                            eventCatMappings.Add(eventCat.DisplayName);
                        }
                        var localDateTime = eventDetails.FirstOrDefault().StartDateTime.DayOfWeek + ", " + eventDetails.FirstOrDefault().StartDateTime.ToString(@"MMM dd, yyyy HH:mm", new CultureInfo("en-US")).ToUpper();
                        if (eventAttribute != null)
                        {
                            localDateTime = eventDetails.FirstOrDefault().StartDateTime.DayOfWeek + ", " + _localTimeZoneConvertProvider.ConvertToLocal(eventDetails.FirstOrDefault().StartDateTime, eventAttribute.TimeZone).ToString(@"MMM dd, yyyy HH:mm", new CultureInfo("en-US")).ToUpper();
                        }

                        return new CategoryEventContainer
                        {
                            CategoryEvent = Mapper.Map<Contracts.Models.CategoryEvent>(ce),
                            EventType = eventObj.EventTypeId.ToString(),
                            EventCategory = EventCategory,
                            City = Mapper.Map<IEnumerable<City>>(cities),
                            State = Mapper.Map<IEnumerable<State>>(states),
                            Country = Mapper.Map<IEnumerable<Country>>(countries),
                            Event = Mapper.Map<Event>(eventObj),
                            EventDetail = Mapper.Map<IEnumerable<EventDetail>>(eventDetails),
                            Rating = Mapper.Map<IEnumerable<Rating>>(eventRatings),
                            CurrencyType = Mapper.Map<CurrencyType>(currencyMapping),
                            Venue = Mapper.Map<IEnumerable<Venue>>(venues),
                            EventTicketAttribute = Mapper.Map<IEnumerable<EventTicketAttribute>>(eventTicketAttributeMapping),
                            EventCategories = eventCatMappings,
                            LocalStartDateTime = localDateTime,
                            TimeZoneAbbrivation = eventAttribute != null ? eventAttribute.TimeZoneAbbreviation : "UTC",
                            EventFrequencyType = eventDetails.FirstOrDefault().EventFrequencyType,
                            ParentCategory = parentCategory,
                            LiveEventDetail = allLiveEventDetails.Where(s => s.EventId == ce.Id).FirstOrDefault()
                        };
                    }
                    catch (Exception e)
                    {
                        return new CategoryEventContainer
                        {
                        };
                    }
                });

                return new FeelCategoryEventQueryResult
                {
                    CategoryEvents = CategoryEventData.ToList(),
                    CountryDescription = Mapper.Map<Contracts.Models.CountryDescription>(countryDescription),
                    CityDescription = cityDescription,
                    CountryContentMapping = Mapper.Map<List<CountryContentMapping>>(countryContents)
                };
            }
            catch (Exception e)
            {
                return new FeelCategoryEventQueryResult
                {
                };
            }
        }
    }
}