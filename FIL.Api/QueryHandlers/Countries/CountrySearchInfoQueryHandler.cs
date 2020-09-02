using FIL.Api.Repositories;
using FIL.Contracts.Queries.Country;
using FIL.Contracts.QueryResults.Country;

namespace FIL.Api.QueryHandlers.Countries
{
    public class CountrySearchInfoQueryHandler : IQueryHandler<CountrySearchInfoQuery, CountrySearchInfoQueryResult>
    {
        private readonly ICountryRepository _countryRepository;

        public CountrySearchInfoQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public CountrySearchInfoQueryResult Handle(CountrySearchInfoQuery query)
        {
            var countryResult = _countryRepository.GetByName(query.Name);
            return new CountrySearchInfoQueryResult
            {
                Id = countryResult.Id,
                AltId = countryResult.AltId,
                Name = countryResult.Name,
                IsoAlphaTwoCode = countryResult.IsoAlphaTwoCode,
                IsoAlphaThreeCode = countryResult.IsoAlphaThreeCode
            };
        }
    }
}