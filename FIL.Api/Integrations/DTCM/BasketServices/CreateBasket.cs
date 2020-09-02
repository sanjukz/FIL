using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.DTCM;
using FIL.Contracts.Models.Integrations.DTCM.BasketCreateOptions;
using FIL.Contracts.Models.Integrations.DTCM.BasketResponse;
using FIL.Contracts.Models.Integrations.HttpHelpers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.DTCM.BasketServices
{
    public interface ICreateBasket : IService
    {
        Task<IResponse<BasketResponse>> CreateBasketAsync(BasketCreateOption basketCreateOption, TokenResponse token);
    }

    public class CreateBasket : Service<BasketResponse>, ICreateBasket
    {
        public CreateBasket(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<BasketResponse>> CreateBasketAsync(BasketCreateOption basketCreateOption, TokenResponse token)
        {
            Urls.BaseUrl = _settings.GetConfigSetting<string>(SettingKeys.Integration.DTCM.BaseUrl);

            IRequestCreateOptions<BasketCreateOption> requestCreateOptions = new RequestCreateOptions<BasketCreateOption>();
            requestCreateOptions.Content = basketCreateOption;
            requestCreateOptions.Header = new Dictionary<string, string>()
            {
                { "Token", $"{token.AccessToken}" }
            };
            requestCreateOptions.UserAgent = "zoonga.com (http://www.zoonga.com/)";

            IHttpResponse httpResponse = await HttpWebRequestProviders<BasketCreateOption>.PostBearerWebRequestProvider(Urls.CreateBasket, requestCreateOptions).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(httpResponse.Response))
            {
                BasketResponse basket = Mapper<BasketResponse>.MapJsonStringToObject(httpResponse.Response);
                return GetResponse(true, basket);
            }
            else
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to create basket", httpResponse.WebException));
            }
            return GetResponse(false, null);
        }
    }
}