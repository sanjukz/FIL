using AutoMapper;
using FIL.Api.Integrations.InfiniteAnalytics;
using FIL.Api.Integrations.InfiniteAnalytics.Recommendation;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Models.Integrations.InfiniteAnalytics.Recommendation;
using FIL.Contracts.Queries.Recommendation;
using FIL.Contracts.QueryResults.Recommendation;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Recommendation
{
    public class RecommendationQueryHandler : IQueryHandler<RecommendationQuery, RecommendationQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IInitSession _initSession;
        private readonly IGetRecommendation _getRecommendation;

        public RecommendationQueryHandler(IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            ICountryRepository countryRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IEventSiteIdMappingRepository eventSiteIdMappingRepository,
            IInitSession initSession,
            IGetRecommendation getRecommendation)
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _initSession = initSession;
            _getRecommendation = getRecommendation;
        }

        public RecommendationQueryResult Handle(RecommendationQuery query)
        {
            var sessionResponseIA = _initSession.GetSession().Result;
            RecommendationModel model = new RecommendationModel
            {
                ClientType = "web_site",
                Count = 4,
                SessionId = sessionResponseIA.Result.SessionId,
                SitePageType = "product_detail",
                siteProductId = query.Id,
            };
            var recommendations = _getRecommendation.GetRecommendations(model).Result;
            var recomendationResult = AutoMapper.Mapper.Map<List<RecommendationItem>>(recommendations.Result.Data[0].Items);
            var recommendationIds = recomendationResult.Select(e => e.SiteProductId);

            var siteEvents = _eventSiteIdMappingRepository.GetBySiteId(query.SiteId).OrderBy(o => o.SortOrder);
            var categoryEvents = _eventRepository.GetByCategoryId(query.EventCategoryId).Where(e => e.IsEnabled == true);
            var categoryEventsForSite = categoryEvents.Where(ce => recommendationIds.Any(se => se == (ce.Id)));
            var eventIds = categoryEventsForSite.Select(ce => ce.Id);
            var eventMapping = _eventRepository.GetByEventIds(eventIds).Where(e => e.IsEnabled == true).ToDictionary(e => e.Id);
            var allEventDetails = _eventDetailRepository.GetByEventIds(eventIds).Where(ed => ed.IsEnabled == true);
            var venueMapping = _venueRepository.GetByVenueIds(allEventDetails.Select(ed => ed.VenueId).Distinct())
                .ToDictionary(v => v.Id);
            var eventDetailsMapping = allEventDetails.GroupBy(ed => ed.EventId)
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
            var CategoryEventData = categoryEventsForSite.Select(ce =>
            {
                var eventObj = eventMapping[ce.Id];
                var eventDetails = eventDetailsMapping[ce.Id];
                var eventDetailIdList = eventDetails.Select(s => s.Id).Distinct();
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
                return new CategoryEventContainer
                {
                    CategoryEvent = Mapper.Map<Contracts.Models.CategoryEvent>(ce),
                    EventType = eventObj.EventTypeId.ToString(),
                    City = Mapper.Map<IEnumerable<City>>(cities),
                    State = Mapper.Map<IEnumerable<State>>(states),
                    Country = Mapper.Map<IEnumerable<Country>>(countries),
                    Event = Mapper.Map<Event>(eventObj),
                    EventDetail = Mapper.Map<IEnumerable<EventDetail>>(eventDetails),
                    CurrencyType = Mapper.Map<CurrencyType>(currencyMapping),
                    Venue = Mapper.Map<IEnumerable<Venue>>(venues),
                    EventTicketAttribute = Mapper.Map<IEnumerable<EventTicketAttribute>>(eventTicketAttributeMapping),
                };
            });

            return new RecommendationQueryResult
            {
                CategoryEvents = CategoryEventData.ToList()
            };
        }
    }
}