using FIL.Api.Repositories;
using FIL.Contracts.Queries.TournamentLayout;
using FIL.Contracts.QueryResults.TournamentLayout;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.TournamentLayouts
{
    public class TournamentSectionDetailsQueryQueryHandler : IQueryHandler<TournamentSectionDetailsQuery, TournamentSectionDetailsQueryResult>
    {
        private ITournamentLayoutRepository _tournamentLayoutRepository;
        private IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private ITournamentLayoutSectionRepository _tournamentLayoutSectionRepository;
        private IMasterVenueLayoutRepository _masterVenueLayoutRepository;
        private IVenueRepository _venueRepository;

        public TournamentSectionDetailsQueryQueryHandler(ITournamentLayoutRepository tournamentLayoutRepository, ITournamentLayoutSectionRepository tournamentLayoutSectionRepository, IMasterVenueLayoutRepository masterVenueLayoutRepository, IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository)
        {
            _tournamentLayoutRepository = tournamentLayoutRepository;
            _tournamentLayoutSectionRepository = tournamentLayoutSectionRepository;
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
            _masterVenueLayoutRepository = masterVenueLayoutRepository;
        }

        public TournamentSectionDetailsQueryResult Handle(TournamentSectionDetailsQuery query)
        {
            var masterVenueLayout = _masterVenueLayoutRepository.GetAllByVenueId(query.VenueId).First().Id;
            if (query.IsTournamentEdit)
            {
                var tournamentLayout = _tournamentLayoutRepository.GetByMasterLayoutAndEventId(masterVenueLayout, query.EventId).First();
                var sectionDetailsByVenueLayout = _masterVenueLayoutSectionRepository.TournamentLayoutSectionsByTournamentLayoutId(tournamentLayout.Id, query.EventId);
                var sectionDetailsByVenueLayoutmasterVenueLayoutSectionId = sectionDetailsByVenueLayout.Select(s => s.IsTournamentExists).ToArray();
                var IsExistTournament = Array.Exists(sectionDetailsByVenueLayoutmasterVenueLayoutSectionId, element => element == 0);

                return new TournamentSectionDetailsQueryResult
                {
                    SectionDetailsByVenueLayout = sectionDetailsByVenueLayout,
                    IsExistTournament = IsExistTournament
                };
            }
            else
            {
                var sectionDetailsByVenueLayout = _masterVenueLayoutSectionRepository.TournamentSectionDetailsByVenueLayout(masterVenueLayout, query.EventId);

                var sectionDetailsByVenueLayoutmasterVenueLayoutSectionId = sectionDetailsByVenueLayout.Select(s => s.IsTournamentExists).ToArray();

                var IsExistTournament = Array.Exists(sectionDetailsByVenueLayoutmasterVenueLayoutSectionId, element => element == 0);

                return new TournamentSectionDetailsQueryResult
                {
                    SectionDetailsByVenueLayout = sectionDetailsByVenueLayout,
                    IsExistTournament = IsExistTournament
                };
            }
        }
    }
}