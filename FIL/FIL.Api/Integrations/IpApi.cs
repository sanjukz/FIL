using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.HttpHelpers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Threading.Tasks;

namespace FIL.Api.Integrations
{
    public interface IIpApi : IService
    {
        Task<IResponse<IpApiResponse>> GetUserLocationByIp(string ip);
    }

    public class IpApi : Service<IpApiResponse>, IIpApi
    {
        public IpApi(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<IpApiResponse>> GetUserLocationByIp(string ip)
        {
            string baseUrl = "http://ip-api.com/json/" + ip;
            IRequestCreateOptions<GetRequestCreateOption> requestCreateOptions = new RequestCreateOptions<GetRequestCreateOption>();
            IHttpResponse httpResponse = await HttpWebRequestProviders<GetRequestCreateOption>.GetWebRequestProviderAsync(baseUrl, requestCreateOptions).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(httpResponse.Response))
            {
                IpApiResponse ipApiResponse = Mapper<IpApiResponse>.MapJsonStringToObject(httpResponse.Response);
                return GetResponse(true, ipApiResponse);
            }
            else
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get session", httpResponse.WebException));
                return GetResponse(false, null);
            }
        }
    }
}