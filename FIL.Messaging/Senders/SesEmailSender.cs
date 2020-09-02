using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Logging;
using FIL.Messaging.Models;
using FIL.Messaging.Models.Emails;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Messaging.Senders
{
    public interface ISesEmailSender : IEmailSender
    {
    }

    public class SesEmailSender : EmailSender, IEmailSender
    {
        public SesEmailSender(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        protected override async Task<IResponse> SendEmailAsync(IEmail email)
        {
            SendTemplatedEmailResponse response;
            IResponse res = new Response { Success = true };
            List<string> ToAddresses = new List<string>();
            using (var client = new AmazonSimpleEmailServiceClient(
                          _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.Ses.AccessKey),
                          _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.Ses.SecretKey),
                          RegionEndpoint.USWest2))
            {
                SendTemplatedEmailRequest sendTemplatedEmail
                    = new SendTemplatedEmailRequest
                    {
                        Source = email.From,
                        Destination = new Destination
                        {
                            ToAddresses =
                        new List<string> { email.To }
                        },
                        Template = "EmailTemplate",
                        TemplateData = JsonConvert.SerializeObject(email.Variables)
                    };

                try
                {
                    response = await client.SendTemplatedEmailAsync(sendTemplatedEmail);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                string result = response.HttpStatusCode.ToString();
                client.Dispose();
                if (result == "OK")
                {
                    return res;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}