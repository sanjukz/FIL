using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.DTCM.PerformanceCreateOption;
using FIL.Contracts.Models.Integrations.DTCM.PerformancePrices;
using FIL.Contracts.Models.Integrations.HttpHelpers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.DTCM.PerformanceServices
{
    public interface IPerformancePrices : IService
    {
        Task<IResponse<PerformancePriceResponse>> PerformancePricesAsync(PerformanceCreateOption performanceCreateOption);
    }

    public abstract class PerformancePrices : Service<PerformancePriceResponse>, IPerformancePrices
    {
        public PerformancePrices(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<PerformancePriceResponse>> PerformancePricesAsync(PerformanceCreateOption performanceCreateOption)
        {
            Urls.BaseUrl = _settings.GetConfigSetting<string>(SettingKeys.Integration.DTCM.BaseUrl);

            IRequestCreateOptions<PerformanceCreateOption> requestCreateOptions = new RequestCreateOptions<PerformanceCreateOption>();
            requestCreateOptions.Header = new Dictionary<string, string>()
            {
                { "Token", $"{performanceCreateOption.AccessToken}" }
            };
            requestCreateOptions.UserAgent = "zoonga.com (http://www.zoonga.com/)";

            IHttpResponse httpResponse = await HttpWebRequestProviders<PerformanceCreateOption>.GetBearerWebRequestProvider(string.Format(Urls.Price, performanceCreateOption.PerformanceCode, performanceCreateOption.Channel, performanceCreateOption.SellerCode), requestCreateOptions).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(httpResponse.Response))
            {
                PerformancePriceResponse performancePrice = Mapper<PerformancePriceResponse>.MapJsonStringToObject(httpResponse.Response);
                return GetResponse(true, performancePrice);
            }
            else
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get performance prices", httpResponse.WebException));
            }
            return GetResponse(false, null);
        }
    }
}