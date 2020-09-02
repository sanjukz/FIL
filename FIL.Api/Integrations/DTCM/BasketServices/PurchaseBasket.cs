using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.DTCM;
using FIL.Contracts.Models.Integrations.DTCM.BasketResponse;
using FIL.Contracts.Models.Integrations.DTCM.PurchaseBasketCreateOptions;
using FIL.Contracts.Models.Integrations.HttpHelpers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.DTCM.BasketServices
{
    public interface IPurchaseBasket : IService
    {
        Task<IResponse<BasketResponse>> PurchaseBasketAsync(PurchaseBasketCreateOption purchaseBasketCreateOption, TokenResponse token);
    }

    public class PurchaseBasket : Service<BasketResponse>, IPurchaseBasket
    {
        public PurchaseBasket(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<BasketResponse>> PurchaseBasketAsync(PurchaseBasketCreateOption purchaseBasketCreateOption, TokenResponse token)
        {
            Urls.BaseUrl = _settings.GetConfigSetting<string>(SettingKeys.Integration.DTCM.BaseUrl);

            IRequestCreateOptions<PurchaseBasketCreateOption> requestCreateOptions = new RequestCreateOptions<PurchaseBasketCreateOption>();
            requestCreateOptions.Content = purchaseBasketCreateOption;
            requestCreateOptions.Header = new Dictionary<string, string>()
            {
                { "Token", $"{token.AccessToken}" }
            };
            requestCreateOptions.UserAgent = "zoonga.com (http://www.zoonga.com/)";

            IHttpResponse httpResponse = await HttpWebRequestProviders<PurchaseBasketCreateOption>.PostBearerWebRequestProvider(string.Format(Urls.PurchaseBasket, purchaseBasketCreateOption.BasketID), requestCreateOptions).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(httpResponse.Response))
            {
                BasketResponse basket = Mapper<BasketResponse>.MapJsonStringToObject(httpResponse.Response);
                return GetResponse(true, basket);
            }
            else
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to purchase basket", httpResponse.WebException));
            }
            return GetResponse(false, null);
        }
    }
}