using FIL.Api.Repositories;
using FIL.Contracts.Queries.TournamentLayout;
using FIL.Contracts.QueryResults.TournamentLayout;
using System;

namespace FIL.Api.QueryHandlers.TournamentLayouts
{
    public class TournamentLayoutSaveSeatQueryHandler : IQueryHandler<TournamentLayoutSaveSeatQuery, TournamentLayoutSaveSeatQueryResult>
    {
        private readonly ITournamentLayoutSectionRepository _tournamentLayoutSectionRepository;
        private readonly ISaveTournamentLayoutSectionRepository _saveTournamentLayoutSectionRepository;
        private readonly ITournamentLayoutSectionSeatRepository _tournamentLayoutSectionSeatRepository;

        public TournamentLayoutSaveSeatQueryHandler(ISaveTournamentLayoutSectionRepository saveTournamentLayoutSectionRepository, ITournamentLayoutSectionRepository tournamentLayoutSectionRepository, ITournamentLayoutSectionSeatRepository tournamentLayoutSectionSeatRepository)
        {
            _saveTournamentLayoutSectionRepository = saveTournamentLayoutSectionRepository;
            _tournamentLayoutSectionRepository = tournamentLayoutSectionRepository;
            _tournamentLayoutSectionSeatRepository = tournamentLayoutSectionSeatRepository;
        }

        public TournamentLayoutSaveSeatQueryResult Handle(TournamentLayoutSaveSeatQuery query)
        {
            try
            {
                var seatCount = _tournamentLayoutSectionSeatRepository.GetSeatCount(query.MasterVenueLayoutSectionId);
                if (seatCount != 0 && query.ShouldSeatInsert)
                {
                    return new TournamentLayoutSaveSeatQueryResult
                    {
                        Success = false,
                        IsExisting = true
                    };
                }
                else
                {
                    var result = _saveTournamentLayoutSectionRepository.SaveSeatLayoutData(query.xmlData, query.MasterVenueLayoutSectionId, query.ShouldSeatInsert);
                    if (result >= 0)
                    {
                        return new TournamentLayoutSaveSeatQueryResult
                        {
                            Success = true,
                            IsExisting = false
                        };
                    }
                    else
                    {
                        return new TournamentLayoutSaveSeatQueryResult
                        {
                            Success = false,
                            IsExisting = false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new TournamentLayoutSaveSeatQueryResult
                {
                    Success = false,
                    IsExisting = false
                };
            }
        }
    }
}