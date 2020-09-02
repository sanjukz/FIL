using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.ValueRetail;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.ValueRetail
{
    public interface IValueRetailAuth : IService
    {
        Task<IResponse<string>> GetToken();
    }

    public class ValueRetailAuth : Service<string>, IValueRetailAuth
    {
        public ValueRetailAuth(ILogger logger, ISettings settings) : base(logger, settings)
        {
        }

        public async Task<IResponse<string>> GetToken()
        {
            var resource = _settings.GetConfigSetting<string>(SettingKeys.Integration.ValueRetail.AuthResource);
            var client_id = _settings.GetConfigSetting<string>(SettingKeys.Integration.ValueRetail.AuthClientId);
            var client_secret = _settings.GetConfigSetting<string>(SettingKeys.Integration.ValueRetail.AuthClientSecret);
            var grant_type = _settings.GetConfigSetting<string>(SettingKeys.Integration.ValueRetail.AuthGrantType);
            var auth_endpoint = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.ValueRetail.AuthEndpoint));

            TokenResponse dataObj = new TokenResponse();

            var tokenRequestData = new Dictionary<string, string>();
            tokenRequestData.Add("resource", resource);
            tokenRequestData.Add("client_id", client_id);
            tokenRequestData.Add("client_secret", client_secret);
            tokenRequestData.Add("grant_type", grant_type);
            /*
            tokenRequestData.Add("resource", "https://valueretail1.onmicrosoft.com/4f5ed29b-4f38-43ef-8980-a9474b8ba757");
            tokenRequestData.Add("client_id", "7b9ecd57-80c4-4a97-a8c9-4451946d3876");
            tokenRequestData.Add("client_secret", "lckehaIl4aJzqQy67JUHshpAitDvcGNK2F+r7U/FCLI=");
            tokenRequestData.Add("grant_type", "client_credentials");
            */
            var authRquest = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/8a865321-d106-4ddf-a46c-cda9f9dceaf5/oauth2/token") { Content = new FormUrlEncodedContent(tokenRequestData) };

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(60);
                    var response = await httpClient.SendAsync(authRquest);
                    string tokenResponse = await response.Content.ReadAsStringAsync();
                    dataObj = Mapper<TokenResponse>.MapFromJson(tokenResponse);
                }

                return GetResponse(true, dataObj.access_token);
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get access token for Value Retail", ex));
                return GetResponse(false, null);
            }
        }
    }
}