using FIL.Api.Repositories;
using FIL.Contracts.Queries.CitySightSeeingLocation;
using FIL.Contracts.QueryResults.CitySightSeeingLocation;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.CitySearch
{
    public class GetCitySightSeeingLocationsQueryHandler : IQueryHandler<GetCitySightSeeingLocationQuery, GetCitySightSeeingLocationQueryResult>
    {
        private readonly ICitySightSeeingLocationRepository _citySightSeeingLocationRepository;

        public GetCitySightSeeingLocationsQueryHandler(ICitySightSeeingLocationRepository citySightSeeingLocationRepository)
        {
            _citySightSeeingLocationRepository = citySightSeeingLocationRepository;
        }

        public GetCitySightSeeingLocationQueryResult Handle(GetCitySightSeeingLocationQuery query)
        {
            var getAllCitySightSeeingLocation = _citySightSeeingLocationRepository.GetAll().Where(s => s.IsEnabled);
            var locationResults = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.CitySightSeeingLocation>>(getAllCitySightSeeingLocation);
            return new GetCitySightSeeingLocationQueryResult
            {
                Locations = locationResults
            };
        }
    }
}