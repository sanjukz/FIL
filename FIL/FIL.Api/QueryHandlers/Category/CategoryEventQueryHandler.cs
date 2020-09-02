using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Category;
using FIL.Contracts.QueryResults.Category;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Category
{
    public class CategoryEventQueryHandler : IQueryHandler<CategoryEventQuery, CategoryEventQueryResult>
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

        public CategoryEventQueryHandler(IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            ICountryRepository countryRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICurrencyTypeRepository currencyTypeRepository)
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
        }

        public CategoryEventQueryResult Handle(CategoryEventQuery query)
        {
            var categoryEventsForSite = _eventRepository.GetByCategoryId(query.EventCategoryId).Where(e => e.IsEnabled).ToList();
            var eventMapping = categoryEventsForSite.ToDictionary(e => e.Id);
            var eventIds = eventMapping.Keys;
            var allEventDetails = _eventDetailRepository.GetByEventIds(eventIds).Where(ed => ed.IsEnabled).ToList();
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
            var currencyTypeList = _currencyTypeRepository.GetAll();
            var CategoryEventData = categoryEventsForSite.Select(ce =>
            {
                var eventObj = eventMapping[ce.Id];
                if (eventDetailsMapping.ContainsKey(ce.Id))
                {
                    var eventDetails = eventDetailsMapping[ce.Id];
                    var eventDetailIdList = eventDetails.Select(s => s.Id).Distinct();
                    var venues = eventDetails.Select(s => s.VenueId).Distinct().Select(v => venueMapping[v]);
                    var cities = venues.Select(s => s.CityId).Distinct().Select(c => cityMapping[c]);
                    var states = cities.Select(s => s.StateId).Distinct().Select(s => stateMapping[s]);
                    var countries = states.Select(s => s.CountryId).Distinct().Select(c => countryMapping[c]);
                    if (!eventDetailIdList.Any())
                    {
                        return new CategoryEventContainer { };
                    }
                    var eventTicketAttributeMapping = new List<Contracts.DataModels.EventTicketAttribute>
                {
                    _eventTicketAttributeRepository.GetMaxPriceByEventDetailId(eventDetailIdList),
                    _eventTicketAttributeRepository.GetMinPriceByEventDetailId(eventDetailIdList)
                };
                    if (eventTicketAttributeMapping.Any())
                    {
                        var currencyMapping = currencyTypeList.Where(s => s.Id == eventTicketAttributeMapping.FirstOrDefault().CurrencyId).FirstOrDefault();
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
                    }
                    else
                    {
                        return new CategoryEventContainer { };
                    }
                }
                else
                {
                    return new CategoryEventContainer
                    {
                    };
                }
            });

            if (CategoryEventData.ToList() != null)
            {
                return new CategoryEventQueryResult
                {
                    CategoryEvents = CategoryEventData.Where(s => s.Event != null).ToList()
                };
            }
            else
            {
                return new CategoryEventQueryResult
                {
                };
            }
        }
    }
}