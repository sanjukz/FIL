using FIL.Api.Repositories;
using FIL.Contracts.Queries.GoogleAPIUtility;
using FIL.Contracts.QueryResults.GoogleAPIUtility;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.GoogleAPIUtility
{
    public class GoogleAPIUtilityQueryHandler : IQueryHandler<GoogleAPIUtilityQuery, GoogleAPIUtilityQueryResult>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;

        public GoogleAPIUtilityQueryHandler(
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            ICountryRepository countryRepository,
            IVenueRepository venueRepository
            )
        {
            _cityRepository = cityRepository;
            _venueRepository = venueRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
        }

        public GoogleAPIUtilityQueryResult Handle(GoogleAPIUtilityQuery query)
        {
            List<FIL.Contracts.Models.GoogleAPIUtility> googleAPIUtilityList = new List<FIL.Contracts.Models.GoogleAPIUtility>();
            if (query.IsAll)
            {
                var city = _cityRepository.SearchByCityName(query.CityName);
                var venues = _venueRepository.GetAllVenueByCity(query.CityName).ToList();

                foreach (FIL.Contracts.DataModels.Venue venue in venues)
                {
                    FIL.Contracts.Models.GoogleAPIUtility googleAPIUtility = new FIL.Contracts.Models.GoogleAPIUtility();
                    var currentCity = _cityRepository.Get(venue.CityId);
                    var currentState = _stateRepository.Get(currentCity.StateId);
                    var currentCountry = _countryRepository.Get(currentState.CountryId);
                    googleAPIUtility.VenueId = venue.Id;
                    googleAPIUtility.Venue = venue.Name;
                    googleAPIUtility.City = currentCity.Name;
                    googleAPIUtility.State = currentState.Name;
                    googleAPIUtility.Country = currentCountry.Name;
                    googleAPIUtilityList.Add(googleAPIUtility);
                }
            }
            else
            {
                var venues = _venueRepository.GetAll().ToList();
                for (int k = 0; k < venues.Count; k = k + 2000)
                {
                    var currentVenues = venues.Skip(k).Take(2000);
                    foreach (FIL.Contracts.DataModels.Venue venue in venues)
                    {
                        FIL.Contracts.Models.GoogleAPIUtility googleAPIUtility = new FIL.Contracts.Models.GoogleAPIUtility();
                        var currentCity = _cityRepository.Get(venue.CityId);
                        var currentState = _stateRepository.Get(currentCity.StateId);
                        var currentCountry = _countryRepository.Get(currentState.CountryId);
                        googleAPIUtility.VenueId = venue.Id;
                        googleAPIUtility.Venue = venue.Name;
                        googleAPIUtility.City = currentCity.Name;
                        googleAPIUtility.State = currentState.Name;
                        googleAPIUtility.Country = currentCountry.Name;
                        googleAPIUtilityList.Add(googleAPIUtility);
                    }
                }
            }
            return new GoogleAPIUtilityQueryResult
            {
                Venues = googleAPIUtilityList
            };
        }
    }
}