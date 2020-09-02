using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.CategoryEventSearch;
using FIL.Contracts.QueryResults.CategoryEventSearch;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.CategoryEventSearch
{
    public class CategoryEventSearchQueryHandler : IQueryHandler<CategoryEventSearchQuery, CategoryEventSearchQueryResult>
    {
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;

        public CategoryEventSearchQueryHandler(IEventSiteIdMappingRepository eventSiteIdMappingRepository, IEventRepository eventRepository,
            IEventCategoryRepository eventCategoryRepository,
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            ICountryRepository countryRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICurrencyTypeRepository currencyTypeRepository)
        {
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
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
        }

        public CategoryEventSearchQueryResult Handle(CategoryEventSearchQuery query)
        {
            List<FIL.Contracts.DataModels.Event> searchResult = _eventRepository.GetFeelEventsByName(query.Name, query.SiteId).ToList();

            if (!string.IsNullOrWhiteSpace(query.CityName) || !string.IsNullOrWhiteSpace(query.CountryName) || !string.IsNullOrWhiteSpace(query.Category) || !string.IsNullOrWhiteSpace(query.SubCategory))
            {
                searchResult = _eventRepository.GetFeelEventsBySearch(query.Name, query.CityName, query.CountryName, query.Category, query.SubCategory, query.SiteId).ToList();
            }

            var eventMapping = searchResult.Where(e => e.IsEnabled).ToDictionary(e => e.Id);
            var allEventDetails = _eventDetailRepository.GetByEventIds(searchResult.Select(e => e.Id))
                .Where(ed => ed.IsEnabled).ToList();
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
            var CategoryEventData = searchResult.Select(ce =>
            {
                var eventObj = eventMapping[ce.Id];
                var eventDetails = eventDetailsMapping[ce.Id];
                var venues = eventDetails.Select(s => s.VenueId).Distinct().Select(v => venueMapping[v]);
                var cities = venues.Select(s => s.CityId).Distinct().Select(c => cityMapping[c]);
                var states = cities.Select(s => s.StateId).Distinct().Select(s => stateMapping[s]);
                var countries = states.Select(s => s.CountryId).Distinct().Select(c => countryMapping[c]);

                var eventCategoryDataModel = _eventCategoryRepository.Get(ce.EventCategoryId);
                var eventCategoryModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryDataModel);

                var eventCategoryParentDataModel = _eventCategoryRepository.Get(eventCategoryModel.EventCategoryId);
                var eventCategoryParentModel = AutoMapper.Mapper.Map<Contracts.Models.EventCategory>(eventCategoryParentDataModel);

                return new CategoryEventContainer
                {
                    CategoryEvent = Mapper.Map<Contracts.Models.CategoryEvent>(ce),
                    EventType = eventObj.EventTypeId.ToString(),
                    City = Mapper.Map<IEnumerable<City>>(cities),
                    State = Mapper.Map<IEnumerable<State>>(states),
                    Country = Mapper.Map<IEnumerable<Country>>(countries),
                    Event = Mapper.Map<Event>(eventObj),
                    EventDetail = Mapper.Map<IEnumerable<EventDetail>>(eventDetails),
                    Venue = Mapper.Map<IEnumerable<Venue>>(venues),
                    ParentCategory = (eventCategoryParentModel != null ? eventCategoryParentModel.DisplayName : ""),
                    EventCategory = (eventCategoryModel != null ? eventCategoryModel.DisplayName : "")
                };
            });
            if (CategoryEventData != null)
            {
                return new CategoryEventSearchQueryResult
                {
                    CategoryEvents = CategoryEventData.ToList()
                };
            }
            else
            {
                return new CategoryEventSearchQueryResult
                {
                };
            }
        }
    }
}