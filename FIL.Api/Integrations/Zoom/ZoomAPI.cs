using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Logging;
using FIL.Logging.Enums;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.Zoom
{
    public interface IZoomAPI : IService
    {
        Task<string> PostAPIDetails(string url, Object obj, string bearerToken);
    }

    public class ZoomAPI : Service<string>, IZoomAPI
    {
        public ZoomAPI(ILogger logger, ISettings settings) : base(logger, settings)
        {
        }

        public async Task<string> PostAPIDetails(string url, Object obj, string bearerToken)
        {
            try
            {
                var endpoint = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.Zoom.Base_Url)) + url;

                string responseData;
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = new TimeSpan(0, 5, 0);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                    var json = JsonConvert.SerializeObject(obj);

                    using (var content = new StringContent(json, Encoding.Default, "application/json"))
                    {
                        using (var response = await httpClient.PostAsync(endpoint, content))
                        {
                            responseData = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
                return responseData;
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get Zoom details " + ex.Message, ex));
                return null;
            }
        }
    }
}