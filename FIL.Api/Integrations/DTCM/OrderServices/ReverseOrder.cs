using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.DTCM.OrderCreateOption;
using FIL.Contracts.Models.Integrations.DTCM.OrderResponse;
using FIL.Contracts.Models.Integrations.HttpHelpers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.DTCM.OrderServices
{
    public interface IReverseOrder : IService
    {
        Task<IResponse<OrderResponse>> ReverseOrderAsync(OrderCreateOption orderCreateOption, IAccessToken accessToken);
    }

    public class ReverseOrder : Service<OrderResponse>, IReverseOrder
    {
        public ReverseOrder(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<OrderResponse>> ReverseOrderAsync(OrderCreateOption orderCreateOption, IAccessToken accessToken)
        {
            Urls.BaseUrl = _settings.GetConfigSetting<string>(SettingKeys.Integration.DTCM.BaseUrl);

            IRequestCreateOptions<OrderCreateOption> requestCreateOptions = new RequestCreateOptions<OrderCreateOption>();
            requestCreateOptions.Content = orderCreateOption;
            requestCreateOptions.Header = new Dictionary<string, string>()
            {
                { "Token", $"{accessToken.Token}" }
            };
            requestCreateOptions.UserAgent = "zoonga.com (http://www.zoonga.com/)";

            IHttpResponse httpResponse = await HttpWebRequestProviders<OrderCreateOption>.PostBearerWebRequestProvider(string.Format(Urls.ReverseOrder, orderCreateOption.OrderId), requestCreateOptions).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(httpResponse.Response))
            {
                OrderResponse orderResponse = Mapper<OrderResponse>.MapJsonStringToObject(httpResponse.Response);
                return GetResponse(true, orderResponse);
            }
            else
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to reverse order", httpResponse.WebException));
            }
            return GetResponse(false, null);
        }
    }
}