using FIL.Configuration;
using FIL.Logging;
using FIL.Messaging.Models;
using FIL.Messaging.Models.Emails;
using System.Threading.Tasks;

namespace FIL.Messaging.Senders
{
    public interface IEmailSender : ISender<IEmail>
    {
    }

    public abstract class EmailSender : Sender<IEmail>, IEmailSender
    {
        protected EmailSender(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public override async Task<IResponse> Send(IEmail email)
        {
            if (IsSendingDisabled)
            {
                return GetResponse();
            }

            return await SendEmailAsync(email);
        }

        protected abstract Task<IResponse> SendEmailAsync(IEmail email);
    }
}