using FIL.Api.Repositories;
using FIL.Contracts.Queries.MatchLayout;
using FIL.Contracts.QueryResults.MatchLayout;
using System.Linq;

namespace FIL.Api.QueryHandlers.MatchLayout
{
    public class MatchLayoutSectionsQueryHandler : IQueryHandler<MatchLayoutSectionDetailsQuery, MatchLayoutSectionDetailsQueryResult>
    {
        private IEventRepository _eventRepository;
        private IVenueRepository _venueRepository;
        private IMasterVenueLayoutRepository _masterVenueLayoutRepository;
        private ITournamentLayoutRepository _tournamentLayoutRepository;
        private ITournamentLayoutSectionRepository _tournamentLayoutSectionRepository;

        public MatchLayoutSectionsQueryHandler(ITournamentLayoutSectionRepository tournamentLayoutSectionRepository, ITournamentLayoutRepository tournamentLayoutRepository, IEventRepository eventRepository, IVenueRepository venueRepository, IMasterVenueLayoutRepository masterVenueLayoutRepository)
        {
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _masterVenueLayoutRepository = masterVenueLayoutRepository;
            _tournamentLayoutRepository = tournamentLayoutRepository;
            _tournamentLayoutSectionRepository = tournamentLayoutSectionRepository;
        }

        public MatchLayoutSectionDetailsQueryResult Handle(MatchLayoutSectionDetailsQuery query)
        {
            var mastervenueLayout = _masterVenueLayoutRepository.GetAllByVenueId(query.VenueId).First();
            var tournamentLayout = _tournamentLayoutRepository.GetByMasterLayoutAndEventId(mastervenueLayout.Id, query.EventId).First();
            var sectionDetailsByTournaments = _tournamentLayoutSectionRepository.SectionDetailsByTournamentLayout(tournamentLayout.Id, query.EventDetailId);

            return new MatchLayoutSectionDetailsQueryResult
            {
                sectionDetailsByTournamentLayout = sectionDetailsByTournaments,
            };
        }
    }
}