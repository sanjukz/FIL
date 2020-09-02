using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.POne;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Globalization;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace FIL.Api.Integrations.POne
{
    public interface IPOneApi : IService
    {
        IResponse<string> GetPOneApiToken();

        Task<IResponse<string>> GetPOneApiData(POneApiRequestModel pOneApiRequestModel);
    }

    public class POneApi : Service<string>, IPOneApi
    {
        public POneApi(ILogger logger, ISettings settings) : base(logger, settings)
        {
        }

        public async Task<IResponse<string>> GetPOneApiData(POneApiRequestModel pOneApiRequestModel)
        {
            pOneApiRequestModel.token = GetPOneApiToken().Result;

            var builder = new UriBuilder($"https://partners.p1hospitality.com/webservice/v1/products/get");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);

            PropertyInfo[] properties = pOneApiRequestModel.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.GetValue(pOneApiRequestModel) != null)
                {
                    if (property.PropertyType == typeof(DateTime?))
                    {
                        var attr = property.Name.ToString().StartsWith("@") ? property.Name.ToString().Substring(1) : property.Name.ToString();
                        query[attr] = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture);
                        continue;
                    }
                    query[property.Name.ToString()] = property.GetValue(pOneApiRequestModel).ToString();
                }
            }

            builder.Query = query.ToString();
            string endpoint = builder.ToString();

            var responseData = string.Empty;

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(5);

                    var response = await httpClient.GetAsync(new Uri(endpoint));
                    responseData = await response.Content.ReadAsStringAsync();
                }

                return GetResponse(true, responseData);
            }
            catch (HttpRequestException ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return GetResponse(false, null);
            }
        }

        public IResponse<string> GetPOneApiToken()
        {
            var token = _settings.GetConfigSetting<string>(Configuration.Utilities.SettingKeys.Integration.POne.Token);

            return GetResponse(true, token);
        }
    }
}