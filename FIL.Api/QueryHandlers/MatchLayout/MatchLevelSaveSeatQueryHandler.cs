using FIL.Api.Repositories;
using FIL.Contracts.Queries.MatchLayout;
using FIL.Contracts.QueryResults.MatchLayout;
using System;

namespace FIL.Api.QueryHandlers.MatchLayout
{
    public class MatchLevelSaveSeatQueryHandler : IQueryHandler<MatchLevelSaveSeatQuery, MatchLevelSaveSeatQueryResult>
    {
        private readonly ITournamentLayoutSectionRepository _tournamentLayoutSectionRepository;
        private readonly ISaveTournamentLayoutSectionRepository _saveTournamentLayoutSectionRepository;
        private readonly ITournamentLayoutSectionSeatRepository _tournamentLayoutSectionSeatRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;

        public MatchLevelSaveSeatQueryHandler(IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository, ISaveTournamentLayoutSectionRepository saveTournamentLayoutSectionRepository, ITournamentLayoutSectionRepository tournamentLayoutSectionRepository, ITournamentLayoutSectionSeatRepository tournamentLayoutSectionSeatRepository)
        {
            _saveTournamentLayoutSectionRepository = saveTournamentLayoutSectionRepository;
            _tournamentLayoutSectionRepository = tournamentLayoutSectionRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _tournamentLayoutSectionSeatRepository = tournamentLayoutSectionSeatRepository;
        }

        public MatchLevelSaveSeatQueryResult Handle(MatchLevelSaveSeatQuery query)
        {
            try
            {
                var seatCount = _matchLayoutSectionSeatRepository.GetSeatCount(query.MasterVenueLayoutSectionId);
                if (seatCount != 0 && query.ShouldSeatInsert)
                {
                    return new MatchLevelSaveSeatQueryResult
                    {
                        Success = false,
                        IsExisting = true
                    };
                }
                else
                {
                    var result = _saveTournamentLayoutSectionRepository.UpdateMatchSeatLayoutData(query.xmlData, query.MasterVenueLayoutSectionId, query.ShouldSeatInsert);
                    if (result >= 0)
                    {
                        return new MatchLevelSaveSeatQueryResult
                        {
                            Success = true,
                            IsExisting = false
                        };
                    }
                    else
                    {
                        return new MatchLevelSaveSeatQueryResult
                        {
                            Success = false,
                            IsExisting = false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new MatchLevelSaveSeatQueryResult
                {
                    Success = false,
                    IsExisting = false
                };
            }
        }
    }
}