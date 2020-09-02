using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace FIL.Api.Integrations
{
    public interface IGoogleMapApi : IService
    {
        Task<IResponse<GoogleMapApiResponse>> GetLocationFromLatLong(string lat, string lon);

        Task<IResponse<GoogleMapApiResponse>> GetLatLongFromAddress(string address);
    }

    public class GoogleMapApi : Service<GoogleMapApiResponse>, IGoogleMapApi
    {
        public GoogleMapApi(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<GoogleMapApiResponse>> GetLocationFromLatLong(string lat, string lon)
        {
            var builder = new UriBuilder("https://maps.googleapis.com/maps/api/geocode/json");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);

            query["latlng"] = string.Format("{0},{1}", lat.Trim(), lon.Trim());
            query["key"] = _settings.GetConfigSetting<string>(SettingKeys.Integration.GoogleGeocoding.APIKey);

            builder.Query = query.ToString();
            string endpoint = builder.ToString();

            string responseData = string.Empty;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(3);
                    var response = await httpClient.GetAsync(new Uri(endpoint));
                    responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                var locationResult = GoogleLocationApiResultParser(responseData);
                locationResult.lat = Convert.ToDouble(lat);
                locationResult.lng = Convert.ToDouble(lon);

                return GetResponse(true, locationResult);
            }
            catch (HttpRequestException ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return GetResponse(false, null);
            }
            catch (Exception ex)
            {
                var errorMsg = new GoogleMapApiError();
                var locationError = Mapper<GoogleMapApiError>.MapJsonStringToObject(responseData);
                _logger.Log(LogCategory.Error, new Exception(locationError.status, ex));
                return GetResponse(false, null);
            }
        }

        public async Task<IResponse<GoogleMapApiResponse>> GetLatLongFromAddress(string address)
        {
            var builder = new UriBuilder("https://maps.googleapis.com/maps/api/geocode/json");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["address"] = address.Trim();
            query["key"] = _settings.GetConfigSetting<string>(SettingKeys.Integration.GoogleGeocoding.APIKey);

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
                locationResult.FullAddress = address;
                return GetResponse(true, locationResult);
            }
            catch (HttpRequestException ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return GetResponse(false, null);
            }
            catch (Exception ex)
            {
                var errorMsg = new GoogleMapApiError();
                var locationError = Mapper<GoogleMapApiError>.MapJsonStringToObject(responseData);
                _logger.Log(LogCategory.Error, new Exception(locationError.status, ex));
                return GetResponse(false, null);
            }
        }

        private GoogleMapApiResponse GoogleLocationApiResultParser(string locationDataString)
        {
            var locationData = Mapper<LocationApiResponse>.MapJsonStringToObject(locationDataString);

            string fullAddress = locationData.results[0].formatted_address,
                city = string.Empty,
                state = string.Empty,
                country = string.Empty,
                name = string.Empty;

            foreach (var address in locationData.results[0].address_components)
            {
                if (address.types.Exists(t => t == "establishment" || t == "point_of_interest" || t == "tourist_attraction"))
                {
                    name = address.long_name != null ? address.long_name : address.short_name;
                    continue;
                }
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
                    if (city == string.Empty)
                    {
                        city = address.long_name;
                    }
                    state = address.long_name;
                    continue;
                }
                if (country == string.Empty && address.types.Exists(t => t == "country"))
                {
                    if (state == string.Empty)
                    {
                        state = address.long_name;
                    }
                    country = address.long_name;
                    continue;
                }
            }

            GoogleMapApiResponse locationResult = new GoogleMapApiResponse
            {
                Name = name,
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