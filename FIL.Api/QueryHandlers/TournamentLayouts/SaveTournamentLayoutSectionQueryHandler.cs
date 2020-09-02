using FIL.Api.Repositories;
using FIL.Contracts.Queries.TournamentLayout;
using FIL.Contracts.QueryResults.TournamentLayout;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.TournamentLayouts
{
    public class SaveTournamentLayoutSectionQueryHandler : IQueryHandler<SaveTournamentLayoutSectionQuery, SaveTournamentLayoutSectionQueryResult>
    {
        private readonly ISaveTournamentLayoutSectionRepository _saveTournamentLayoutSectionRepository;
        private readonly ITournamentLayoutSectionRepository _tournamentLayoutSectionRepository;

        public SaveTournamentLayoutSectionQueryHandler(ISaveTournamentLayoutSectionRepository saveTournamentLayoutSectionRepository, ITournamentLayoutSectionRepository tournamentLayoutSectionRepository)
        {
            _saveTournamentLayoutSectionRepository = saveTournamentLayoutSectionRepository;
            _tournamentLayoutSectionRepository = tournamentLayoutSectionRepository;
        }

        public SaveTournamentLayoutSectionQueryResult Handle(SaveTournamentLayoutSectionQuery query)
        {
            try
            {
                var tournament = _tournamentLayoutSectionRepository.GetAll();
                var data = tournament.Select(s => s.MasterVenueLayoutSectionId).ToArray();
                var tournamentSections = query.SectionIds.Split(',').ToArray();
                var tournamentdata = Array.ConvertAll(tournamentSections, int.Parse);
                var difference = tournamentdata.Except(data);
                var sectionData = string.Join(",", difference);

                var result = _saveTournamentLayoutSectionRepository.SaveTournamentLayoutSectionData(query.EventId, query.VenueId, query.SectionIds, query.CreatedBy);
                if (result == 1)
                {
                    return new SaveTournamentLayoutSectionQueryResult
                    {
                        Success = true,
                        IsExisting = false,
                    };
                }
                else
                {
                    return new SaveTournamentLayoutSectionQueryResult
                    {
                        Success = false,
                        IsExisting = false,
                    };
                }
            }
            catch (Exception ex)
            {
                return new SaveTournamentLayoutSectionQueryResult
                {
                    Success = false,
                    IsExisting = false,
                };
            }
        }
    }
}