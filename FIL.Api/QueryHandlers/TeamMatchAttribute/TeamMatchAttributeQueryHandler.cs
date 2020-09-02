using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.TeamMatchAttribute;
using FIL.Contracts.QueryResults;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.States
{
    public class TeamMatchAttributeQueryHandler : IQueryHandler<TeamMatchAttributeQuery, TeamMatchAttributeQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IMatchAttributeRepository _matchAttributeRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;

        public TeamMatchAttributeQueryHandler(
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            ITeamRepository teamRepository,
            IMatchAttributeRepository matchAttributeRepository,
            IVenueRepository venueRepository,
            ICountryRepository countryRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository)
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _teamRepository = teamRepository;
            _matchAttributeRepository = matchAttributeRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
        }

        public TeamMatchAttributeQueryResult Handle(TeamMatchAttributeQuery query)
        {
            var eventId = _eventRepository.GetByAltId(query.EventAltId);
            var allEventDetails = _eventDetailRepository.GetSubEventByEventId(eventId.Id).Where(ed => ed.IsEnabled == true).OrderBy(o => o.StartDateTime);
            var matchAttribute = _matchAttributeRepository.GetByEventDetailIds(allEventDetails.Select(ed => ed.Id).Distinct());
            var team = _teamRepository.GetAll();
            var venues = _venueRepository.GetByVenueIds(allEventDetails.Select(ed => ed.VenueId).Distinct());
            var cities = _cityRepository.GetByCityIds(venues.Select(ed => ed.CityId).Distinct());

            return new TeamMatchAttributeQueryResult
            {
                EventDetail = Mapper.Map<IEnumerable<EventDetail>>(allEventDetails),
                MatchAttribute = Mapper.Map<IEnumerable<MatchAttribute>>(matchAttribute),
                Team = Mapper.Map<IEnumerable<Team>>(team),
                Venue = Mapper.Map<IEnumerable<Venue>>(venues),
                City = Mapper.Map<IEnumerable<City>>(cities),
            };
        }
    }
}