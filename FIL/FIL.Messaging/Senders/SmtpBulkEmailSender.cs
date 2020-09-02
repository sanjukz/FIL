using FIL.Configuration;
using FIL.Logging;
using FIL.Messaging.Models;
using FIL.Messaging.Models.Emails;
using System;
using System.Threading.Tasks;

namespace FIL.Messaging.Senders
{
    public interface ISmtpBulkEmailSender : IBulkEmailSender
    {
    }

    public class SmtpBulkEmailSender : BulkEmailSender, ISmtpBulkEmailSender
    {
        public SmtpBulkEmailSender(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        protected override Task<IResponse> SendBulkEmail(IBulkEmail emails)
        {
            throw new NotImplementedException();
        }
    }
}