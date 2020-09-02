using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Logging;
using FIL.Logging.Enums;
using FIL.Messaging.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Messaging.Senders
{
    public interface IGupShupTextMessageSender : ITextMessageSender
    {
    }

    public class GupShupTextMessageSender : TextMessageSender, IGupShupTextMessageSender
    {
        public GupShupTextMessageSender(ILogger logger, ISettings settings)
           : base(logger, settings)
        {
        }

        protected override async Task<IResponse> SendTextMessage(string to, string from, string body)
        {
            var apiUrl = _settings.GetConfigSetting<string>(SettingKeys.Messaging.TextMessages.GupShup.ApiUrlFormat);
            var userId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.TextMessages.GupShup.UserId);
            var password = _settings.GetConfigSetting<string>(SettingKeys.Messaging.TextMessages.GupShup.Password);

            try
            {
                using (var client = new HttpClient())
                {
                    var httpRequest = await client.GetAsync(string.Format(apiUrl, to, body, userId, password)).ConfigureAwait(false);
                    var message = httpRequest.IsSuccessStatusCode ? null : $"GupShup text message failed to send with status {httpRequest.StatusCode}";
                    return GetResponse(httpRequest.IsSuccessStatusCode, message);
                }
            }
            catch (Exception ex)
            {
                var customEx = new Exception("GupShup text message failed to send", ex);
                _logger.Log(LogCategory.Error, customEx);
                return GetResponse(false, ex.Message);
            }
        }
    }
}