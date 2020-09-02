using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Logging;
using FIL.Messaging.Models;
using FIL.Messaging.Models.Emails;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FIL.Messaging.Senders
{
    public interface ISmtpEmailSender : IEmailSender
    {
    }

    public class SmtpEmailSender : EmailSender, ISmtpEmailSender
    {
        public SmtpEmailSender(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        protected override async Task<IResponse> SendEmailAsync(IEmail email)
        {
            IResponse res = new Response();
            string Subject = email.Subject;
            string strBody = email.MailBody;
            MailMessage mail = new MailMessage(email.From, email.To);
            mail.Subject = Subject;
            if (email.IsAttachment)
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(email.pdfdata, 0, email.pdfdata.Length);
                ms.Position = 0;
                mail.Attachments.Add(new System.Net.Mail.Attachment(ms, "e-ticket_" + DateTime.UtcNow.ToString() + ".pdf"));
            }

            if (!string.IsNullOrWhiteSpace(email.Bcc))
                mail.Bcc.Add(email.Bcc);
            mail.IsBodyHtml = true;
            mail.Body = strBody;
            try
            {
                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(_settings.GetConfigSetting<string>(SettingKeys.Messaging.Email.AmazonSES.EmailHost), Convert.ToInt32(_settings.GetConfigSetting<string>((SettingKeys.Messaging.Email.AmazonSES.EmailPort)))))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(_settings.GetConfigSetting<string>(SettingKeys.Messaging.Email.AmazonSES.HostUserName), _settings.GetConfigSetting<string>(SettingKeys.Messaging.Email.AmazonSES.HostUserPwd));
                    client.EnableSsl = true;
                    client.Send(mail);
                }
                mail.Dispose();
                res.Success = true;
            }
            catch (Exception ex)
            {
                mail.Dispose();
                res.Success = false;
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
            return res;
        }
    }
}