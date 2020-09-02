using FIL.Api.Repositories;
using FIL.Contracts.Queries.CitySearch;
using FIL.Contracts.QueryResults.CitySearch;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.CitySearch
{
    public class CitySearchQueryHandler : IQueryHandler<CitySearchQuery, CitySearchQueryResult>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;

        public CitySearchQueryHandler(ICityRepository cityRepository, IStateRepository stateRepository)
        {
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
        }

        public CitySearchQueryResult Handle(CitySearchQuery query)
        {
            var searchResult = _cityRepository.SearchByCityName(query.Name);
            var cityResult = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.City>>(searchResult);
            return new CitySearchQueryResult
            {
                Cities = cityResult,
            };
        }
    }
}