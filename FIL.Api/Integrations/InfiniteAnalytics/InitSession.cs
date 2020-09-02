using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.HttpHelpers;
using FIL.Contracts.Models.Integrations.InfiniteAnalytics;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.InfiniteAnalytics
{
    public interface IInitSession : IService
    {
        Task<IResponse<SessionResponse>> GetSession();
    }

    public class InitSession : Service<SessionResponse>, IInitSession
    {
        public InitSession(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<SessionResponse>> GetSession()
        {
            Urls.BaseUrl = "https://feelaplace.infiniteanalytics.com/SocialGenomix";//_settings.GetConfigSetting<string>(SettingKeys.Integration.InfiniteAnalytics.BaseUrl);

            IRequestCreateOptions<GetRequestCreateOption> requestCreateOptions = new RequestCreateOptions<GetRequestCreateOption>();
            IHttpResponse httpResponse = await HttpWebRequestProviders<GetRequestCreateOption>.GetWebRequestProviderAsync(Urls.InfiniteAnalytics.Session, requestCreateOptions).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(httpResponse.Response))
            {
                SessionResponse token = Mapper<SessionResponse>.MapJsonStringToObject(httpResponse.Response);
                return GetResponse(true, token);
            }
            else
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get session", httpResponse.WebException));
                return GetResponse(false, null);
            }
        }
    }
}