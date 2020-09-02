using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.ValueRetail;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace FIL.Api.Integrations.ValueRetail
{
    public interface IValueRetailAPI : IService
    {
        Task<IResponse<string>> GetValueRetailAPIData(ValueRetailCommanRequestModel valueRetailCommanRequestModel, string apiRoute, string productName);
    }

    public class ValueRetailAPI : Service<string>, IValueRetailAPI
    {
        private IValueRetailAuth _valueRetailAuth;

        public ValueRetailAPI(ILogger logger, ISettings settings, IValueRetailAuth valueRetailAuth) : base(logger, settings)
        {
            _valueRetailAuth = valueRetailAuth;
        }

        public async Task<IResponse<string>> GetValueRetailAPIData(ValueRetailCommanRequestModel valueRetailCommanRequestModel, string apiRoute, string productName)
        {
            var _accessToken = await _valueRetailAuth.GetToken();
            var builder = new UriBuilder($"https://data0integration0prep0neu.azure-api.net/opdconnect/api/{productName}/{apiRoute}");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);

            PropertyInfo[] properties = valueRetailCommanRequestModel.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.GetValue(valueRetailCommanRequestModel) != null)
                {
                    if (property.PropertyType == typeof(DateTime?))
                    {
                        var attr = property.Name.ToString().StartsWith("@") ? property.Name.ToString().Substring(1) : property.Name.ToString();
                        query[attr] = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture);
                        continue;
                    }

                    query[property.Name.ToString()] = property.GetValue(valueRetailCommanRequestModel).ToString();
                }
            }
            query["aggregatorId"] = "SDK";
            query["otaId"] = "INT00065";

            builder.Query = query.ToString();
            string endpoint = builder.ToString();

            var responseData = string.Empty;

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", "8ace1302001848ed9a311fca09ef8909");
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Result);

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
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to fetch Chauffeur Driven data for Value Retail", ex));
                return GetResponse(false, null);
            }
        }
    }
}