using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.FeelSearch;
using FIL.Contracts.QueryResults.Category;
using FIL.Contracts.QueryResults.FeelSearch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.FeelSearch
{
    public class FeelSearchQueryHandler : IQueryHandler<FeelSearchQuery, FeelSearchQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IRatingRepository _ratingRepository;

        public FeelSearchQueryHandler(IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository,
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            ICountryRepository countryRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IRatingRepository ratingRepository,
            IEventSiteIdMappingRepository eventSiteIdMappingRepository,
            IEventCategoryMappingRepository eventCategoryMappingRepository)
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _ratingRepository = ratingRepository;
        }

        public FeelSearchQueryResult Handle(FeelSearchQuery query)
        {
            if (query.IsAdvanceSearch)
                return AdvanceSearch(query);
            else
                return Search(query);
        }

        private FeelSearchQueryResult Search(FeelSearchQuery query)
        {
            var cityDetails = _cityRepository.GetFeelSearchCityByName(query.Name).ToList();
            var stateDetails = _stateRepository.GetFeelSearchStateByName(query.Name).ToList();
            var countryDetails = _countryRepository.GetFeelSearchCountryByName(query.Name).ToList();

            return new FeelSearchQueryResult
            {
                Cities = cityDetails,
                States = stateDetails,
                Countries = countryDetails
            };
        }

        private FeelSearchQueryResult AdvanceSearch(FeelSearchQuery query)
        {
            var siteEvents = _eventSiteIdMappingRepository
                .GetBySiteId(query.SiteId)
                .OrderBy(o => o.SortOrder).ToList();

            List<Contracts.DataModels.Event> searchEvents = new List<Contracts.DataModels.Event>();
            if (query.PlaceAltIds != null && query.PlaceAltIds.Count() > 0)
            {
                searchEvents = _eventRepository.GetEventsByAltIds(query.PlaceAltIds)
                   .ToList();
            }
            else
            {
                searchEvents = _eventRepository.GetFeelEventsBySearchString(query.Name, query.SiteId)
                   .ToList();
            }

            var eventMapping = searchEvents.ToDictionary(e => e.Id);
            var eventIds = eventMapping.Keys;
            var eventCategoryMappings = _eventCategoryMappingRepository.GetByEventIds(eventIds).Where(ed => ed.IsEnabled == true);
            var allEventDetails = _eventDetailRepository.GetByEventIds(eventIds).Where(ed => ed.IsEnabled == true);
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
            var cityMapping = _cityRepository.GetByCityIds(cityIds)
                .ToDictionary(c => c.Id);
            var stateId = cityMapping.Values.Select(c => c.StateId).Distinct();
            var stateMapping = _stateRepository.GetByStateIds(stateId)
                .ToDictionary(s => s.Id);
            var countryIdList = stateMapping.Values.Select(s => s.CountryId).Distinct();
            var countryMapping = _countryRepository.GetByCountryIds(countryIdList)
                .ToDictionary(c => c.Id);
            var eventsBySortOrder = new List<FIL.Contracts.DataModels.Event>();
            foreach (var item in siteEvents)
            {
                var events = searchEvents.FirstOrDefault(w => w.Id == item.EventId);
                if (events != null)
                {
                    eventsBySortOrder.Add(AutoMapper.Mapper.Map<FIL.Contracts.DataModels.Event>(events));
                }
            }
            var CategoryEventData = eventsBySortOrder.Select(ce =>
            {
                try
                {
                    var eventObj = eventMapping[ce.Id];
                    var eventDetails = eventDetailsMapping[ce.Id];
                    var eventCategoryMap = eventCategoryMapping[ce.Id];
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
                    var venues = eventDetails.Select(s => s.VenueId).Distinct().Select(v => venueMapping[v]);
                    var cities = venues.Select(s => s.CityId).Distinct().Select(c => cityMapping[c]);
                    var states = cities.Select(s => s.StateId).Distinct().Select(s => stateMapping[s]);
                    var countries = states.Select(s => s.CountryId).Distinct().Select(c => countryMapping[c]);
                    var eventTicketAttributeMapping = new List<Contracts.DataModels.EventTicketAttribute>
                {
                    _eventTicketAttributeRepository.GetMaxPriceByEventDetailId(eventDetailIdList),
                    _eventTicketAttributeRepository.GetMinPriceByEventDetailId(eventDetailIdList)
                };
                    var currencyMapping = _currencyTypeRepository.GetByCurrencyId(eventTicketAttributeMapping.First().CurrencyId);
                    List<string> eventCatMappings = new List<string>();

                    foreach (var eventCat in eventCategoryMap)
                    {
                        var eventCategoryDataModel = _eventCategoryRepository.Get(eventCat.EventCategoryId);
                        var eventCategoryModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryDataModel);
                        eventCatMappings.Add(eventCategoryModel.DisplayName);
                    }
                    return new CategoryEventContainer
                    {
                        CategoryEvent = Mapper.Map<Contracts.Models.CategoryEvent>(ce),
                        EventType = eventObj.EventTypeId.ToString(),
                        EventCategory = eventObj.EventCategoryId.ToString(),
                        City = Mapper.Map<IEnumerable<City>>(cities),
                        State = Mapper.Map<IEnumerable<State>>(states),
                        Country = Mapper.Map<IEnumerable<Country>>(countries),
                        Event = Mapper.Map<Event>(eventObj),
                        EventDetail = Mapper.Map<IEnumerable<EventDetail>>(eventDetails),
                        Rating = Mapper.Map<IEnumerable<Rating>>(eventRatings),
                        CurrencyType = Mapper.Map<CurrencyType>(currencyMapping),
                        Venue = Mapper.Map<IEnumerable<Venue>>(venues),
                        EventTicketAttribute = Mapper.Map<IEnumerable<EventTicketAttribute>>(eventTicketAttributeMapping),
                        EventCategories = eventCatMappings
                    };
                }
                catch (Exception e)
                {
                    return new CategoryEventContainer
                    {
                    };
                }
            });
            return new FeelSearchQueryResult
            {
                FeelAdvanceSearchQueryResult = new FeelCategoryEventQueryResult
                {
                    CategoryEvents = CategoryEventData.ToList()
                }
            };
        }
    }
}