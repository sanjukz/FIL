using FIL.Api.Repositories;
using FIL.Contracts.Queries.Country;
using FIL.Contracts.QueryResults.Country;

namespace FIL.Api.QueryHandlers.Countries
{
    public class CountryGetQueryHandler : IQueryHandler<CountryGetQuery, CountryGetQueryResult>
    {
        private readonly ICountryRepository _countryRepository;

        public CountryGetQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public CountryGetQueryResult Handle(CountryGetQuery query)
        {
            var countryDataModel = _countryRepository.GetByAltId(query.AltId);
            var countryModel = AutoMapper.Mapper.Map<Contracts.Models.Country>(countryDataModel);

            return new CountryGetQueryResult
            {
                Countries = AutoMapper.Mapper.Map<Contracts.Models.Country>(countryModel),
            };
        }
    }
}