using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Logging;
using FIL.Logging.Enums;
using FIL.Messaging.Models;
using FIL.Messaging.Models.Emails;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Messaging.Senders
{
    public interface IConfirmationEmailSender : IEmailSender
    {
    }

    public class ConfirmationEmailSender : EmailSender, IConfirmationEmailSender
    {
        public ConfirmationEmailSender(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        protected override async Task<IResponse> SendEmailAsync(IEmail email)
        {
            IResponse res = new Response();
            using (var client = new AmazonSimpleEmailServiceClient(
                          _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.Ses.AccessKey),
                          _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.Ses.SecretKey),
                          RegionEndpoint.USWest2))
            {
                var sendTemplatedEmail = new SendTemplatedEmailRequest
                {
                    Source = email.From,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string>
                        {
                            email.To
                        },
                        BccAddresses = email.Bcc == null ? new List<string>() : new List<string> { email.Bcc },
                    },
                    Template = email.TemplateName,
                    ConfigurationSetName = String.IsNullOrEmpty(email.ConfigurationSetName) ? "" : email.ConfigurationSetName,
                    TemplateData = JsonConvert.SerializeObject(email.Variables)
                };

                SendTemplatedEmailResponse response;
                try
                {
                    response = await client.SendTemplatedEmailAsync(sendTemplatedEmail);
                    var result = response.HttpStatusCode.ToString();
                    if (result == "OK")
                    {
                        res.Success = true;
                    }
                    else
                    {
                        res.ErrorMessage = result;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogCategory.Error, ex);
                    res.Success = false;
                }
                return res;
            }
        }
    }
}