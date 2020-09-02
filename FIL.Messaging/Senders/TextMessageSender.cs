using FIL.Configuration;
using FIL.Logging;
using FIL.Messaging.Models;
using FIL.Messaging.Models.TextMessages;
using FIL.Utilities.Extensions;
using System.Threading.Tasks;

namespace FIL.Messaging.Senders
{
    public interface ITextMessageSender : ISender<ITextMessage>
    {
    }

    public abstract class TextMessageSender : Sender<ITextMessage>, ITextMessageSender
    {
        protected TextMessageSender(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public override async Task<IResponse> Send(ITextMessage textMessage)
        {
            if (IsSendingDisabled)
            {
                return GetResponse();
            }

            if (!textMessage.Forced && textMessage.To.IsNullOrBlank()) // TODO: Add opt-in at user level for canspam
            {
                return GetResponse(false, "Phone number invalid or has opted out of text messaging.");
            }

            var from = textMessage.From.IsNullOrBlank() ? _settings.GetConfigSetting<string>("") : textMessage.From;

            return await SendTextMessage(textMessage.To, from, textMessage.Body);
        }

        protected abstract Task<IResponse> SendTextMessage(string to, string from, string body);
    }
}