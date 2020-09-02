using FIL.Api.Repositories;
using FIL.Contracts.Queries.CountryPlace;
using FIL.Contracts.QueryResults.CountryPlace;
using System.Linq;

namespace FIL.Api.QueryHandlers.CountryPlace
{
    public class StatePlaceQueryHandler : IQueryHandler<StatePlaceQuery, StatePlaceQueryResult>
    {
        private readonly IStateRepository _stateRepository;

        public StatePlaceQueryHandler(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public StatePlaceQueryResult Handle(StatePlaceQuery query)
        {
            var statePlaces = _stateRepository.GetAllStatePlaceByCountry(query.CountryName);

            return new StatePlaceQueryResult
            {
                StatePlaces = statePlaces.ToList()
            };
        }
    }
}