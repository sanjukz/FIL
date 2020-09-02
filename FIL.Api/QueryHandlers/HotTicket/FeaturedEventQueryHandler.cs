using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.HotTicket;
using FIL.Contracts.QueryResults;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.HotTicket
{
    public class FeaturedEventQueryHandler : Profile, IQueryHandler<FeaturedEventQuery, FeaturedEventQueryResults>
    {
        private readonly IFeaturedEventRepository _featuredEventRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;

        public FeaturedEventQueryHandler(IFeaturedEventRepository featuredEventRepository,
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IVenueRepository venueRepository,
            ICountryRepository countryRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICurrencyTypeRepository currencyTypeRepository)
        {
            _featuredEventRepository = featuredEventRepository;
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

        public FeaturedEventQueryResults Handle(FeaturedEventQuery query)
        {
            var featuredEventsForSite = _featuredEventRepository.GetBySiteIds(query.SiteId).OrderBy(fe => fe.SortOrder);
            var eventIds = featuredEventsForSite.Select(fe => fe.EventId);

            var eventMapping = _eventRepository.GetByEventIds(eventIds).Where(e => e.IsEnabled == true).ToDictionary(e => e.Id);
            var allEventDetails = _eventDetailRepository.GetByEventIds(eventIds).Where(ed => ed.IsEnabled == true).OrderBy(o => o.StartDateTime);
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

            var featuredEventData = featuredEventsForSite.Select(fe =>
            {
                /*var eventObj = eventMapping[fe.EventId];
                var eventDetails = eventDetailsMapping[fe.EventId];
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
                CurrencyType currencyType = new CurrencyType();
                if (eventObj.Id == 15439|| eventObj.Id == 9466)
                {
                    currencyType = Mapper.Map <CurrencyType>(_currencyTypeRepository.GetByCurrencyId(7));
                }
                else
                {
                    currencyType = Mapper.Map<CurrencyType>(_currencyTypeRepository.GetByCurrencyId(eventTicketAttributeMapping.First().CurrencyId));
                } */
                return new FeaturedEventContainer
                {
                    /*FeaturedEvent = Mapper.Map<FeaturedEvent>(fe),
                    EventType = eventObj.EventTypeId.ToString(),
                    City = Mapper.Map<IEnumerable<City>>(cities),
                    State = Mapper.Map<IEnumerable<State>>(states),
                    Country = Mapper.Map<IEnumerable<Country>>(countries),
                    Event = Mapper.Map<Event>(eventObj),
                    EventDetail = Mapper.Map<IEnumerable<EventDetail>>(eventDetails),
                    CurrencyType = Mapper.Map<CurrencyType>(currencyType),
                    Venue = Mapper.Map<IEnumerable<Venue>>(venues),
                    EventTicketAttribute = Mapper.Map<IEnumerable<EventTicketAttribute>>(eventTicketAttributeMapping),*/
                    //EventTicketDetail = Mapper.Map<IEnumerable<EventTicketDetail>>(eventTicketDetailMapping),
                };
            });
            return new FeaturedEventQueryResults
            {
                FeaturedEvents = featuredEventData.ToList()
            };
        }
    }
}