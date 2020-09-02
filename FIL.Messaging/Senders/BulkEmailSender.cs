using FIL.Configuration;
using FIL.Logging;
using FIL.Messaging.Models;
using FIL.Messaging.Models.Emails;
using System.Threading.Tasks;

namespace FIL.Messaging.Senders
{
    public interface IBulkEmailSender : ISender<IBulkEmail>
    {
    }

    public abstract class BulkEmailSender : Sender<IBulkEmail>, IBulkEmailSender
    {
        protected BulkEmailSender(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public override async Task<IResponse> Send(IBulkEmail emails)
        {
            if (IsSendingDisabled)
            {
                return GetResponse();
            }

            return await SendBulkEmail(emails);
        }

        protected abstract Task<IResponse> SendBulkEmail(IBulkEmail emails);
    }
}