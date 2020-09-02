using FIL.Api.Repositories;
using FIL.Contracts.Queries.MatchLayout;
using FIL.Contracts.QueryResults.MatchLayout;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.MatchLayout
{
    public class MatchLayoutGetTournamentQueryHandler : IQueryHandler<MatchLayoutGetTournamentQuery, MatchLayoutGetTournamentQueryResult>
    {
        private readonly ITournamentLayoutRepository _tournamentLayoutRepository;
        private readonly IEventRepository _eventRepository;

        public MatchLayoutGetTournamentQueryHandler(IMasterVenueLayoutRepository masterVenueLayoutRepository, ITournamentLayoutRepository tournamentLayoutRepository, IEventRepository eventRepository)
        {
            _tournamentLayoutRepository = tournamentLayoutRepository;
            _eventRepository = eventRepository;
        }

        public MatchLayoutGetTournamentQueryResult Handle(MatchLayoutGetTournamentQuery query)
        {
            var tournamentLayouts = _tournamentLayoutRepository.GetAll();
            var events = _eventRepository.GetByAllEventIds(tournamentLayouts.Select(s => s.EventId));
            return new MatchLayoutGetTournamentQueryResult
            {
                events = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Event>>(events)
            };
        }
    }
}