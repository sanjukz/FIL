using FIL.Api.Repositories;
using FIL.Contracts.Queries.CountryPlace;
using FIL.Contracts.QueryResults.CountryPlace;
using System.Linq;

namespace FIL.Api.QueryHandlers.CountryPlace
{
    public class CountryPlaceQueryHandler : IQueryHandler<CountryPlaceQuery, CountryPlaceQueryResult>
    {
        private readonly ICountryRepository _countryRepository;

        public CountryPlaceQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public CountryPlaceQueryResult Handle(CountryPlaceQuery query)
        {
            var countryPlaces = _countryRepository.GetAllCountryPlace();
            var categoryPlaceCount = _countryRepository.GetAllCountryPlaceCountByEventCategoryId(query.ParentCategoryId);
            return new CountryPlaceQueryResult
            {
                CountryPlaces = countryPlaces.ToList(),
                CountryCategoryCounts = categoryPlaceCount.ToList()
            };
        }
    }
}