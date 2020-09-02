using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.AdvancedSearch;
using FIL.Contracts.QueryResults.AdvancedSearch;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.AdvancedSearch
{
    public class AdvancedSearchQueryHandler : IQueryHandler<AdvancedSearchQuery, AdvancedSearchQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepositor;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;

        public AdvancedSearchQueryHandler(IEventRepository eventRepository,
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
            _eventDetailRepositor = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _currencyTypeRepository = currencyTypeRepository;
        }

        public AdvancedSearchQueryResult Handle(AdvancedSearchQuery query)
        {
            var searchEventForSite = _eventRepository.GetAll();
            var SearchEventResultData = searchEventForSite.Select(ce =>
            {
                var eventMapping = _eventRepository.GetByEventId(ce.Id);
                var eventDetailMapping = _eventDetailRepositor.GetSubEventByEventId(ce.Id);
                var eventDetailIdList = eventDetailMapping.Select(s => s.Id).Distinct();
                var eventTicketDetailMapping = _eventTicketDetailRepository.GetByEventDetailIds(eventDetailIdList);
                var eventTicketDetailIdList = eventTicketDetailMapping.Select(s => s.Id).Distinct();
                var eventTicketAttributeMapping = _eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetailIdList);
                var CurrencyIdList = eventTicketAttributeMapping.Select(s => s.CurrencyId).Distinct();
                var CurrencyMapping = _currencyTypeRepository.GetByCurrencyId(CurrencyIdList.FirstOrDefault());
                var venueIdList = eventDetailMapping.Select(s => s.VenueId).Distinct();
                var venueMapping = _venueRepository.GetByVenueIds(venueIdList);
                var cityIdList = venueMapping.Select(s => s.CityId).Distinct();
                var cityMapping = _cityRepository.GetByCityIds(cityIdList);
                var stateIdList = cityMapping.Select(s => s.StateId).Distinct();
                var stateMapping = _stateRepository.GetByStateIds(stateIdList);
                var countryIdList = stateMapping.Select(s => s.CountryId).Distinct();
                var countryMapping = _countryRepository.GetByCountryIds(countryIdList);
                return new AdvancedSearchContainer
                {
                    EventType = ((EventType)eventMapping.EventTypeId).ToString(),
                    City = Mapper.Map<IEnumerable<Contracts.Models.City>>(cityMapping),
                    State = Mapper.Map<IEnumerable<Contracts.Models.State>>(stateMapping),
                    Country = Mapper.Map<IEnumerable<Contracts.Models.Country>>(countryMapping),
                    Event = Mapper.Map<Contracts.Models.Event>(eventMapping),
                    EventDetail = Mapper.Map<IEnumerable<Contracts.Models.EventDetail>>(eventDetailMapping),
                    CurrencyType = Mapper.Map<Contracts.Models.CurrencyType>(CurrencyMapping),
                    Venue = Mapper.Map<IEnumerable<Contracts.Models.Venue>>(venueMapping),
                    EventTicketAttribute = Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(eventTicketAttributeMapping),
                    EventTicketDetail = Mapper.Map<IEnumerable<Contracts.Models.EventTicketDetail>>(eventTicketDetailMapping),
                };
            });
            List<AdvancedSearchContainer> searcheventresult = SearchEventResultData.ToList();
            return new AdvancedSearchQueryResult
            {
                SearchResultEvents = searcheventresult
            };
        }
    }
}