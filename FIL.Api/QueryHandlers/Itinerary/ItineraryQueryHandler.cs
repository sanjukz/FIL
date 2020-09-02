using FIL.Api.Repositories;
using FIL.Contracts.Queries.Itinerary;
using FIL.Contracts.QueryResults.Itinerary;
using System.Linq;

namespace FIL.Api.QueryHandlers.Export
{
    public class ItineraryQueryHandler : IQueryHandler<ItineraryQuery, ItineraryQueryResult>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;

        public ItineraryQueryHandler(ICityRepository cityRepository, IStateRepository stateRepository)
        {
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
        }

        public ItineraryQueryResult Handle(ItineraryQuery query)
        {
            var result = _cityRepository.GetAllFeelCityAndCountries();
            var stateResult = _stateRepository.GetAllFeelStateAndCountries();
            return new ItineraryQueryResult
            {
                ItinerarySearchData = result.ToList(),
                FeelStateData = stateResult.ToList()
            };
        }
    }
}