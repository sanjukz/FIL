using FIL.Api.Repositories;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.CitySightSeeing;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CitySightSeeing
{
    public class GetAllLocationCommandHandler : BaseCommandHandlerWithResult<GetAllLocationCommand, GetAllLocationCommandResult>
    {
        private readonly ICitySightSeeingLocationRepository _citySightSeeingLocationRepository;
        private readonly ICitySightSeeingEventDetailMappingRepository _citySightSeeingEventDetailMappingRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ILogger _logger;
        private readonly ISettings _settings;

        public GetAllLocationCommandHandler(
        ICitySightSeeingLocationRepository citySightSeeingLocationRepository, ICountryRepository countryRepository, IStateRepository stateRepository, IVenueRepository venueRepository, ISettings settings, ICityRepository cityRepository, ICitySightSeeingEventDetailMappingRepository citySightSeeingEventDetailMappingRepository, ILogger logger,
        IMediator mediator) : base(mediator)
        {
            _logger = logger;
            _citySightSeeingLocationRepository = citySightSeeingLocationRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _venueRepository = venueRepository;
            _settings = settings;
            _cityRepository = cityRepository;
            _citySightSeeingEventDetailMappingRepository = citySightSeeingEventDetailMappingRepository;
        }

        public async Task<string> Get(object obj)
        {
            var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.Endpoint));

            string responseData;
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.Timeout = new TimeSpan(1, 0, 0);

                string auth = string.Format(_settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestIdentifier) + ":" + _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.Token));

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-request-authentication", _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestAuthentication));

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-request-identifier", _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.RequestIdentifier));

                var json = JsonConvert.SerializeObject(obj);

                using (var content = new StringContent(json, System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("booking_service", content))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return responseData;
        }

        protected override async Task<ICommandResult> Handle(GetAllLocationCommand command)
        {
            await FetchAndSaveLocation(); // Getting all locations & saving
            GetAllLocationCommandResult locations = new GetAllLocationCommandResult();
            var disabledallCitySightSeeingEventDetails = _citySightSeeingEventDetailMappingRepository.GetAll();
            var disabledallCitySightSeeingEventDetailsList = AutoMapper.Mapper.Map<IEnumerable<CitySightSeeingEventDetailMapping>>(disabledallCitySightSeeingEventDetails);
            foreach (var citySightSeeingEventDetailMappingData in disabledallCitySightSeeingEventDetailsList)
            {
                var tempCitySightSeeingEventDetail = _citySightSeeingEventDetailMappingRepository.GetByCitySightSeeingTicketId(citySightSeeingEventDetailMappingData.CitySightSeeingTicketId);
                tempCitySightSeeingEventDetail.IsEnabled = false;
                _citySightSeeingEventDetailMappingRepository.Save(tempCitySightSeeingEventDetail);
            }
            var getCitySightSeeingLocations = _citySightSeeingLocationRepository.GetAll().Where(s => s.IsEnabled);
            locations.Locations = getCitySightSeeingLocations.ToList();
            return locations;
        }

        public async Task<IEnumerable<Example>> GetResponse(string country)
        {
            HttpClient webClient = new HttpClient();
            Uri uri = new Uri("https://restcountries.eu/rest/v2/all");
            HttpResponseMessage response = await webClient.GetAsync(uri);
            var jsonString = await response.Content.ReadAsStringAsync();
            var _Data = Mapper<IList<Example>>.MapFromJson(jsonString);
            var filteredCountry = _Data.Where(s => s.name == country);
            return filteredCountry;
        }

        private async Task FetchAndSaveLocation()
        {
            //Disabling all Locations
            var disabledAllLocations = _citySightSeeingLocationRepository.GetAll();
            var disabledAllLocationsModel = AutoMapper.Mapper.Map<IEnumerable<CitySightSeeingLocation>>(disabledAllLocations);
            foreach (var locationData in disabledAllLocationsModel)
            {
                var getLocation = _citySightSeeingLocationRepository.Get(locationData.Id);
                getLocation.IsEnabled = false;
                _citySightSeeingLocationRepository.Save(getLocation);
            }

            CitySightSeeingApi input = new CitySightSeeingApi();
            input.data = new RequestData();
            input.request_type = "location_list";
            input.data.distributor_id = _settings.GetConfigSetting<string>(SettingKeys.Integration.CitySightSeeing.DistributorId);
            LocationList objData = new LocationList();
            objData = Mapper<LocationList>.MapFromJson((await Get(input)));
            foreach (FIL.Contracts.Models.CitySightSeeing.Location location in objData.data.locations)
            {
                try
                {
                    var country = _countryRepository.GetByName(location.country_name);
                    if (country == null)
                    {
                        var getCountryCode = await GetResponse(location.country_name);
                        var alphatwocode = location.country_name.Substring(0, 1);
                        var alphathreecode = location.country_name.Substring(0, 2);
                        if (getCountryCode != null && getCountryCode.Count() > 0)
                        {
                            var CountryCodes = getCountryCode.ToList();
                            alphatwocode = CountryCodes[0].alpha2Code;
                            alphathreecode = CountryCodes[0].alpha3Code;
                        }
                        country = _countryRepository.Save(new Country
                        {
                            Name = location.country_name,
                            IsoAlphaTwoCode = alphatwocode,
                            IsoAlphaThreeCode = alphathreecode,
                            IsEnabled = true,
                        });
                    }

                    var state = _stateRepository.GetByNameAndCountryId(location.location_name, country.Id);
                    if (state == null)
                    {
                        state = _stateRepository.Save(new State
                        {
                            Name = location.location_name,
                            CountryId = country.Id,
                            IsEnabled = true,
                        });
                    }

                    var city = _cityRepository.GetByNameAndStateId(location.location_name, state.Id);
                    if (city == null)
                    {
                        city = _cityRepository.Save(new City
                        {
                            Name = location.location_name,
                            StateId = state.Id,
                            IsEnabled = true,
                        });
                    }

                    var citySightSeeingLocation = _citySightSeeingLocationRepository.GetByName(location.location_name);
                    if (citySightSeeingLocation == null)
                    {
                        citySightSeeingLocation = _citySightSeeingLocationRepository.Save(new CitySightSeeingLocation
                        {
                            AltId = Guid.NewGuid(),
                            Name = location.location_name,
                            CountryName = location.country_name,
                            ModifiedBy = Guid.NewGuid(),
                            CreatedUtc = DateTime.UtcNow,
                            IsEnabled = true
                        });
                    }
                    else
                    {
                        citySightSeeingLocation.IsEnabled = true;
                        _citySightSeeingLocationRepository.Save(citySightSeeingLocation);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogCategory.Error, new Exception("Failed to Sync HOHO Location Data", ex));
                    continue;
                }
            }
        }
    }
}