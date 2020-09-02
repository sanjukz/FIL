using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Logging;
using FIL.Messaging.Models;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FIL.Messaging.Senders
{
    public interface ITwilioTextMessageSender : ITextMessageSender
    {
    }

    public class TwilioTextMessageSender : TextMessageSender, ITwilioTextMessageSender
    {
        public TwilioTextMessageSender(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        protected override async Task<IResponse> SendTextMessage(string to, string from, string body)
        {
            try
            {
                TwilioClient.Init(_settings.GetConfigSetting<string>(SettingKeys.Messaging.TextMessages.Twilio.AccountSid), _settings.GetConfigSetting<string>(SettingKeys.Messaging.TextMessages.Twilio.Token));

                // TODO: ApplicationSid? MessagingServiceSid?
                var message = await MessageResource.CreateAsync(
                    new PhoneNumber(to),
                    messagingServiceSid: _settings.GetConfigSetting<string>(SettingKeys.Messaging.TextMessages.Twilio.ServiceSid),
                    body: body);

                return GetResponse(!message.ErrorCode.HasValue, message.ErrorMessage);
            }
            catch (Exception ex)
            {
                //_logger.Log(LogCategory.Error, new Exception("Failed to send SMS", ex));
                return GetResponse(false, ex.Message);
            }
        }
    }
}