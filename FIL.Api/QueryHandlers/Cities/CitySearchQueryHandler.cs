using FIL.Api.Repositories;
using FIL.Contracts.Queries.City;
using FIL.Contracts.QueryResults;

namespace FIL.Api.QueryHandlers.Cities
{
    public class CitySearchQueryHandler : IQueryHandler<CitySearchQuery, CitySearchQueryResult>
    {
        private readonly ICityRepository _cityRepository;

        public CitySearchQueryHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public CitySearchQueryResult Handle(CitySearchQuery query)
        {
            var cityResult = _cityRepository.GetByName(query.Name);
            return new CitySearchQueryResult
            {
                IsExisting = cityResult != null,
            };
        }
    }
}