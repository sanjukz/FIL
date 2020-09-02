using FIL.Api.Repositories;
using FIL.Contracts.Queries.Country;
using FIL.Contracts.QueryResults.Country;

namespace FIL.Api.QueryHandlers.Countries
{
    public class CountrySearchQueryHandler : IQueryHandler<CountrySearchQuery, CountrySearchQueryResult>
    {
        private readonly ICountryRepository _countryRepository;

        public CountrySearchQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public CountrySearchQueryResult Handle(CountrySearchQuery query)
        {
            var countryResult = _countryRepository.GetByName(query.Name);
            return new CountrySearchQueryResult
            {
                IsExisting = countryResult != null,
            };
        }
    }
}