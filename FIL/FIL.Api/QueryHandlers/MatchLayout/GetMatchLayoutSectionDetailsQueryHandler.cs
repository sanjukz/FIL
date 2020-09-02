using FIL.Api.Repositories;
using FIL.Contracts.Queries.MatchLayout;
using FIL.Contracts.QueryResults.MatchLayout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.MatchLayout
{
    public class GetMatchLayoutSectionDetailsQueryHandler : IQueryHandler<GetMatchLayoutSectionDetailsQuery, GetMatchLayoutSectionDetailsQueryResult>
    {
        private IMatchLayoutRepository _matchLayoutRepository;
        private IMatchLayoutSectionRepository _matchLayoutSectionRepository;
        private ITournamentLayoutRepository _tournamentLayoutRepository;
        private IMasterVenueLayoutRepository _masterVenueLayoutRepository;
        private ITournamentLayoutSectionRepository _tournamentLayoutSectionRepository;
        private IEventFeeTypeMappingRepository _eventFeeTypeMappingRepository;
        private IEntryGateRepository _entryGateRepository;
        private ITicketCategoryRepository _ticketCategoryRepository;

        public GetMatchLayoutSectionDetailsQueryHandler(IMatchLayoutRepository matchLayoutRepository, IMatchLayoutSectionRepository matchLayoutSectionRepository, ITournamentLayoutRepository tournamentLayoutRepository, IMasterVenueLayoutRepository masterVenueLayoutRepository, ITournamentLayoutSectionRepository tournamentLayoutSectionRepository, IEventFeeTypeMappingRepository eventFeeTypeMappingRepository, IEntryGateRepository entryGateRepository, ITicketCategoryRepository ticketCategoryRepository)
        {
            _masterVenueLayoutRepository = masterVenueLayoutRepository;
            _matchLayoutRepository = matchLayoutRepository;
            _matchLayoutSectionRepository = matchLayoutSectionRepository;
            _tournamentLayoutRepository = tournamentLayoutRepository;
            _tournamentLayoutSectionRepository = tournamentLayoutSectionRepository;
            _eventFeeTypeMappingRepository = eventFeeTypeMappingRepository;
            _entryGateRepository = entryGateRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
        }

        public GetMatchLayoutSectionDetailsQueryResult Handle(GetMatchLayoutSectionDetailsQuery query)
        {
            var mastervenuelayout = _masterVenueLayoutRepository.GetAllByVenueId(query.VenueId).First();
            var tournamentLayout = _tournamentLayoutRepository.GetByMasterLayoutAndEventId(mastervenuelayout.Id, query.EventId).First();
            var eventFeeTypeMappings = _eventFeeTypeMappingRepository.GetByEventId(query.EventId);
            var entryGates = _entryGateRepository.GetAll();
            var ticketCategotirs = _ticketCategoryRepository.GetAll();
            if (query.IsMatchEdit)
            {
                var matchLayout = _matchLayoutRepository.GetByTournamentLayoutId(tournamentLayout.Id).First();
                var sectionDetailsByVenueLayout = _matchLayoutSectionRepository.GetByEventDetailId(query.EventDetailId);
                var sectionDetailsByVenueLayoutTournamentSectionId = sectionDetailsByVenueLayout.Select(s => s.IsMatchExists).ToArray();
                var IsExistMatch = Array.Exists(sectionDetailsByVenueLayoutTournamentSectionId, element => element == 0);

                return new GetMatchLayoutSectionDetailsQueryResult
                {
                    SectionDetailsByMatchLayout = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.SectionDetailsByMatchLayout>>(sectionDetailsByVenueLayout),
                    IsExistMatch = IsExistMatch,
                    // TicketCategories = AutoMapper.Mapper.Map<List<Kz.Contracts.Models.TicketCategory>>(ticketCategotirs),
                    EntryGates = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EntryGates>>(entryGates)
                };
            }
            else
            {
                return new GetMatchLayoutSectionDetailsQueryResult
                {
                    SectionDetailsByMatchLayout = null,
                    IsExistMatch = false
                };
            }
        }
    }
}