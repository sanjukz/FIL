using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.POne;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.POne
{
    public interface IPOneBooking : IService
    {
        Task<IResponse<string>> POneBookingApi(POneBookingModel pOneOrder);

        IResponse<string> GetPOneApiToken();
    }

    public class POneBooking : Service<string>, IPOneBooking
    {
        public POneBooking(ILogger logger, ISettings settings) : base(logger, settings)
        {
        }

        public async Task<IResponse<string>> POneBookingApi(POneBookingModel pOneOrder)
        {
            var apiToken = GetPOneApiToken().Result;

            var builder = new UriBuilder($"http://test.partners.p1hospitality.com/webservice/v1/orders/create?token={apiToken}");
            builder.Port = -1;
            string endpoint = builder.ToString();
            var responseData = string.Empty;

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    var response = await httpClient.PostAsJsonAsync(endpoint, pOneOrder);
                    responseData = await response.Content.ReadAsStringAsync();
                }
                //var res = Mapper<LocationApiResponse>.MapJsonStringToObject(responseData);
                return GetResponse(true, responseData);
            }
            catch (Exception ex)
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