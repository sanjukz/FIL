using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.ASI
{
    public interface IASIApi : IService
    {
        Task<IResponse<string>> GetAsiDetails(string url);
    }

    public class ASIApi : Service<string>, IASIApi
    {
        public ASIApi(ILogger logger, ISettings settings) : base(logger, settings)
        {
        }

        public async Task<IResponse<string>> GetAsiDetails(string url)
        {
            try
            {
                string _address = url;
                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(_address);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return GetResponse(true, result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get ASI details", ex));
                return GetResponse(false, null);
            }
        }
    }
}