using FIL.Logging;
using FIL.Configuration;
using FIL.Contracts.Enums;
using System;
using FIL.Contracts.DataModels;
using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.Models.Integrations;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;

namespace FIL.Web.Feel.Providers
{
    public interface ILatLongProvider
    {
        Task<GoogleMapApiResponse> GetLatLong(string address);
    }

    public class LatLongProvider : ILatLongProvider
    {
        private readonly FIL.Logging.ILogger _logger;
        public LatLongProvider(ILogger logger, ISettings settings
            )
        {
            _logger = logger;
        }

        public async Task<GoogleMapApiResponse> GetLatLong(string address)
        {
            try
            {
                var googleService = await GetLatLongFromAddress(address);
                if (googleService != null && googleService.lat != null && googleService.lng != null)
                {
                    return googleService;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return null;
            }
        }

        public async Task<GoogleMapApiResponse> GetLatLongFromAddress(string address)
        {
            var builder = new UriBuilder("https://maps.googleapis.com/maps/api/geocode/json");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["address"] = address.Trim();
            query["key"] = "AIzaSyDZ4Ik7ENzLWY1tLh1ul8NxhWBdWGK6tQU";

            builder.Query = query.ToString();
            string endpoint = builder.ToString();

            string responseData = string.Empty;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(3);
                    var response = await httpClient.GetAsync(new Uri(endpoint));
                    responseData = await response.Content.ReadAsStringAsync();
                }
                var locationResult = GoogleLocationApiResultParser(responseData);
                return locationResult;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                var errorMsg = new GoogleMapApiError();
                return null;
            }
        }

        private GoogleMapApiResponse GoogleLocationApiResultParser(string locationDataString)
        {
            var locationData = JsonConvert.DeserializeObject<LocationApiResponse>(locationDataString);

            string fullAddress = locationData.results[0].formatted_address,
                city = string.Empty,
                state = string.Empty,
                country = string.Empty;

            foreach (var address in locationData.results[0].address_components)
            {
                if (address.types.Exists(t => t == "locality"))
                {
                    city = address.long_name;
                    continue;
                }
                if (city == string.Empty && address.types.Exists(t => t == "sublocality"))
                {
                    city = address.long_name;
                    continue;
                }
                if (city == string.Empty && address.types.Exists(t => t == "postal_town"))
                {
                    city = address.long_name;
                    continue;
                }
                if (city == string.Empty && address.types.Exists(t => t == "sublocality_level_1"))
                {
                    city = address.long_name;
                    continue;
                }
                if (city == string.Empty && address.types.Exists(t => t == "sublocality_level_2"))
                {
                    city = address.long_name;
                    continue;
                }
                if (city == string.Empty && address.types.Exists(t => t == "sublocality_level_3"))
                {
                    city = address.long_name;
                    continue;
                }
                if (city == string.Empty && address.types.Exists(t => t == "sublocality_level_4"))
                {
                    city = address.long_name;
                    continue;
                }
                if (city == string.Empty && address.types.Exists(t => t == "sublocality_level_5"))
                {
                    city = address.long_name;
                    continue;
                }
                if (city == string.Empty && address.types.Exists(t => t == "administrative_area_level_3"))
                {
                    city = address.long_name;
                    continue;
                }
                if (state == string.Empty && address.types.Exists(t => t == "administrative_area_level_2"))
                {
                    state = address.long_name;
                    continue;
                }

                if (address.types.Exists(t => t == "administrative_area_level_1"))
                {
                    state = address.long_name;
                    continue;
                }
                if (country == string.Empty && address.types.Exists(t => t == "country"))
                {
                    country = address.long_name;
                    continue;
                }
            }

            GoogleMapApiResponse locationResult = new GoogleMapApiResponse
            {
                FullAddress = fullAddress,
                CityName = city,
                StateName = state,
                CountryName = country,
                lat = locationData.results[0].geometry.location.lat,
                lng = locationData.results[0].geometry.location.lng
            };
            return locationResult;
        }
    }
}
