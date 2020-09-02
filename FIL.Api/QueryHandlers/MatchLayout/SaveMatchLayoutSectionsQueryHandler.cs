using FIL.Api.Repositories;
using FIL.Contracts.Queries.MatchLayout;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Api.QueryHandlers.MatchLayout
{
    public class SaveMatchLayoutSectionsQueryHandler : IQueryHandler<SaveMatchLayoutSectionQuery, SaveMatchLayoutSectionQueryResult>
    {
        private readonly ISaveMatchLayoutSectionRepository _saveMatchLayoutSectionRepository;
        private readonly ITournamentLayoutSectionRepository _tournamentLayoutSectionRepository;
        private readonly IMatchLayoutSectionRepository _matchLayoutSectionRepository;

        public SaveMatchLayoutSectionsQueryHandler(ISaveMatchLayoutSectionRepository saveMatchLayoutSectionRepository, IMatchLayoutSectionRepository matchLayoutSectionRepository)
        {
            _saveMatchLayoutSectionRepository = saveMatchLayoutSectionRepository;
            _matchLayoutSectionRepository = matchLayoutSectionRepository;
        }

        public SaveMatchLayoutSectionQueryResult Handle(SaveMatchLayoutSectionQuery query)
        {
            try
            {
                var result = _saveMatchLayoutSectionRepository.SaveMatchLayoutSectionData(query.sectionData, query.EventId, query.VenueId, query.EventDetailId, query.CreatedBy, query.feeDetails);

                if (true)
                {
                    return new SaveMatchLayoutSectionQueryResult
                    {
                        Success = true,
                        IsExisting = false,
                    };
                }
                else
                {
                    return new SaveMatchLayoutSectionQueryResult
                    {
                        Success = false,
                        IsExisting = true,
                    };
                }
            }
            catch (Exception ex)
            {
                return new SaveMatchLayoutSectionQueryResult
                {
                    Success = false,
                    IsExisting = false,
                };
            }
        }
    }
}