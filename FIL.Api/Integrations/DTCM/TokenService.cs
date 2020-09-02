using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.DTCM;
using FIL.Contracts.Models.Integrations.HttpHelpers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.DTCM
{
    public interface ITokenService : IService
    {
        Task<IResponse<TokenResponse>> CreateTokenAsync();
    }

    public abstract class TokenService : Service<TokenResponse>, ITokenService
    {
        public TokenService(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<TokenResponse>> CreateTokenAsync()
        {
            Urls.BaseUrl = _settings.GetConfigSetting<string>(SettingKeys.Integration.DTCM.BaseUrl);

            TokenCreateOption tokenCreateOptions = new TokenCreateOption();
            tokenCreateOptions.grant_type = "client_credentials";

            IRequestCreateOptions<TokenCreateOption> requestCreateOptions = new RequestCreateOptions<TokenCreateOption>();
            requestCreateOptions.Content = tokenCreateOptions;
            requestCreateOptions.Accept = "application/vnd.softix.api-v1.0+json";
            requestCreateOptions.Header = new Dictionary<string, string>()
            {
                { "Auth", $"{_settings.GetConfigSetting<string>(SettingKeys.Integration.DTCM.ClientKey)}:{_settings.GetConfigSetting<string>(SettingKeys.Integration.DTCM.ServerKey)}"}
            };
            requestCreateOptions.UserAgent = "zoonga.com (http://www.zoonga.com/)";

            IHttpResponse httpResponse = await HttpWebRequestProviders<TokenCreateOption>.PostWebRequestProviderAsync(Urls.Token, requestCreateOptions).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(httpResponse.Response))
            {
                TokenResponse token = Mapper<TokenResponse>.MapJsonStringToObject(httpResponse.Response);
                return GetResponse(true, token);
            }
            else
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to create token", httpResponse.WebException));
            }
            return GetResponse(false, null);
        }
    }
}