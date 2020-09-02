using FIL.Api.Repositories;
using FIL.Contracts.Queries.Description;
using FIL.Contracts.QueryResults.Description;
using System.Linq;

namespace FIL.Api.QueryHandlers.CitySearch
{
    public class DescriptionQueryHandler : IQueryHandler<DescriptionQuery, DescriptionQueryResult>
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICityDescriptionRepository _cityDescriptionRepository;
        private readonly ICountryDescriptionRepository _countryDescription;
        private readonly IStateDescriptionRepository _stateDescriptionRepository;

        public DescriptionQueryHandler(ICityRepository cityRepository,
            ICityDescriptionRepository cityDescriptionRepository,
            ICountryDescriptionRepository countryDescriptionRepository,
            IStateDescriptionRepository stateDescriptionRepository,
            ICountryRepository countryRepository)
        {
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _cityDescriptionRepository = cityDescriptionRepository;
            _countryDescription = countryDescriptionRepository;
            _stateDescriptionRepository = stateDescriptionRepository;
        }

        public DescriptionQueryResult Handle(DescriptionQuery query)
        {
            var city = new FIL.Contracts.DataModels.City();
            var country = new FIL.Contracts.DataModels.Country();
            var cityDescription = new FIL.Contracts.DataModels.CityDescription();
            var countryDescription = new FIL.Contracts.DataModels.CountryDescription();
            var stateDescription = new FIL.Contracts.DataModels.StateDescription();
            if (query.IsCountryDescription && !query.IsCityDescription && !query.IsStateDescription)
            {
                country = _countryRepository.GetFeelCountryByName(query.Name).ToList().FirstOrDefault();
            }
            else if (!query.IsCountryDescription && query.IsCityDescription && !query.IsStateDescription)
            {
                city = _cityRepository.GetFeelCityByName(query.Name).ToList().FirstOrDefault();
            }
            else if (!query.IsCountryDescription && !query.IsCityDescription && query.IsStateDescription)
            {
                stateDescription = _stateDescriptionRepository.GetByStateId(query.StateId);
            }
            if (country != null && !query.IsCityDescription && !query.IsStateDescription)
            {
                countryDescription = _countryDescription.GetByCountryId(country.Id);
            }
            else if (city != null && !query.IsCountryDescription && !query.IsStateDescription)
            {
                cityDescription = _cityDescriptionRepository.GetBycityId(city.Id);
            }
            return new DescriptionQueryResult
            {
                City = city,
                Country = country,
                CityDescription = cityDescription,
                CountryDescription = countryDescription,
                StateDescription = stateDescription
            };
        }
    }
}