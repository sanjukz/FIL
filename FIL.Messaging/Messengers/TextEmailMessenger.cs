using FIL.Messaging.Models;
using FIL.Messaging.Models.TextMessages;
using FIL.Messaging.Senders;
using System.Threading.Tasks;

namespace FIL.Messaging.Messengers
{
    public class TextEmailMessenger : IMessenger<TestEmailMessageRequest>
    {
        private readonly ITwilioTextMessageSender _twilioSender;

        public TextEmailMessenger(ITwilioTextMessageSender twilioSender)
        {
            _twilioSender = twilioSender;
        }

        public Task<IResponse> Send(TestEmailMessageRequest request)
        {
            return _twilioSender.Send(new TextMessage
            {
                Body = request.Text,
                To = request.To
            });
        }
    }
}