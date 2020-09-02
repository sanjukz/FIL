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
    public interface IKapTextMessageSender : ITextMessageSender
    {
    }

    public class KapTextMessageSender : TextMessageSender, IKapTextMessageSender
    {
        public KapTextMessageSender(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        protected override async Task<IResponse> SendTextMessage(string to, string from, string body)
        {
            var apiUrl = _settings.GetConfigSetting<string>(SettingKeys.Messaging.TextMessages.Kap.ApiUrlFormat);
            var apiKey = _settings.GetConfigSetting<string>(SettingKeys.Messaging.TextMessages.Kap.ApiKey);
            var kapSender = _settings.GetConfigSetting<string>(SettingKeys.Messaging.TextMessages.Kap.Sender);

            try
            {
                using (var client = new HttpClient())
                {
                    var httpRequest = await client.GetAsync(string.Format(apiUrl, apiKey, to, kapSender, body)).ConfigureAwait(false);
                    var message = httpRequest.IsSuccessStatusCode ? null : $"KAP text message failed to send with status {httpRequest.StatusCode}";
                    return GetResponse(httpRequest.IsSuccessStatusCode, message);
                }
            }
            catch (Exception ex)
            {
                var customEx = new Exception("KAP text message failed to send", ex);
                _logger.Log(LogCategory.Error, customEx);
                return GetResponse(false, ex.Message);
            }
        }
    }
}