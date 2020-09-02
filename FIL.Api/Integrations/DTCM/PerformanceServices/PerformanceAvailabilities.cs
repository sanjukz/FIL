using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.DTCM.PerformanceAvailabilities;
using FIL.Contracts.Models.Integrations.DTCM.PerformanceCreateOption;
using FIL.Contracts.Models.Integrations.HttpHelpers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.DTCM.PerformanceServices
{
    public interface IPerformanceAvailabilityServicee : IService
    {
        Task<IResponse<PerformanceAvailabilityResponse>> PerformanceAvailabilityAsync(PerformanceCreateOption performanceCreateOption);
    }

    public abstract class PerformanceAvailabilities : Service<PerformanceAvailabilityResponse>, IPerformanceAvailabilityServicee
    {
        public PerformanceAvailabilities(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<PerformanceAvailabilityResponse>> PerformanceAvailabilityAsync(PerformanceCreateOption performanceCreateOption)
        {
            Urls.BaseUrl = _settings.GetConfigSetting<string>(SettingKeys.Integration.DTCM.BaseUrl);

            IRequestCreateOptions<PerformanceCreateOption> requestCreateOptions = new RequestCreateOptions<PerformanceCreateOption>();
            requestCreateOptions.Header = new Dictionary<string, string>()
            {
                { "Token", $"{performanceCreateOption.AccessToken}" }
            };
            requestCreateOptions.UserAgent = "zoonga.com (http://www.zoonga.com/)";

            IHttpResponse httpResponse = await HttpWebRequestProviders<PerformanceCreateOption>.GetBearerWebRequestProvider(string.Format(Urls.Availability, performanceCreateOption.PerformanceCode, performanceCreateOption.Channel, performanceCreateOption.SellerCode), requestCreateOptions).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(httpResponse.Response))
            {
                PerformanceAvailabilityResponse performanceAvailability = Mapper<PerformanceAvailabilityResponse>.MapJsonStringToObject(httpResponse.Response);
                return GetResponse(true, performanceAvailability);
            }
            else
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get performance availabilities", httpResponse.WebException));
            }
            return GetResponse(false, null);
        }
    }
}