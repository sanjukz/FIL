using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Feel.ViewModels.Invite;
using FIL.Contracts.Commands.Invite;
using Microsoft.AspNetCore.Mvc;
using FIL.Http;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Web.Core.UrlsProvider;
using FIL.Messaging.Models.Emails;
using FIL.Messaging.Senders;

namespace FIL.Web.Feel.Controllers
{
    public class UserInviteController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ISessionProvider _sessionProvider;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly ISettings _settings;
        private readonly ISiteUrlsProvider _siteUrlsProvider;
        private readonly IAccountEmailSender _emailSender;
        public UserInviteController(ICommandSender commandSender, IQuerySender querySender, ISessionProvider sessionProvider,
            ISiteUrlsProvider siteUrlsProvider, ISiteIdProvider siteIdProvider, ISettings settings, IAccountEmailSender emailSender)

        {
            _commandSender = commandSender;
            _querySender = querySender;
            _sessionProvider = sessionProvider;
            _settings = settings;
            _siteIdProvider = siteIdProvider;
            _siteUrlsProvider = siteUrlsProvider;
            _emailSender = emailSender;
        }



        [HttpPost]
        [Route("api/invite/sendinvite")]
        public async Task<UserInviteResponseViewModel> SendInvite([FromBody]UserInviteRequestViewModel request)
        {
            if (ModelState.IsValid)
            {
                var session = await _sessionProvider.Get();
                var result = new { Success = true };
                try
                {
                    var userinvite = new UserInviteCommand()
                    {
                        UserEmail = request.email,
                        UserInviteCode = RandomString(6)
                    };
                    await _commandSender.Send(userinvite);

                    sendEmail(userinvite);

                    return new UserInviteResponseViewModel
                    {
                        Success = result.Success,
                    };
                }
                catch (Exception ex)
                {
                    return new UserInviteResponseViewModel
                    {
                        Success = false
                    };
                }
            }
            else
            {
                return new UserInviteResponseViewModel
                {
                    Success = false
                };
            }
        }

        private void sendEmail(UserInviteCommand model)
        {
            try
            {
                string siteUrls = _siteUrlsProvider.GetSiteUrl(_siteIdProvider.GetSiteId());
                Email email = new Email();
                email.To = model.UserEmail;
                email.Bcc = "accounts@kyazoonga.com";
                email.From = "FeelitLIVE  <no-reply@feelitLIVE.com>";
                email.TemplateName = "FeelInviteeRegistration";

                email.Variables = new Dictionary<string, object>
                {
                    ["signinlink"] = "<a href='" + siteUrls + "/signup" + "' style='margin-right:100px; '>" + "<h3>feelitLIVE.com</h3></a>",
                    ["userpassword"] = model.UserInviteCode
                };
                _emailSender.Send(email);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        [Route("api/invite/getconfig")]
        public async Task<bool> getInviteConfig(string setname)
        {
            var invite = _settings.GetConfigSetting("IsWebsiteInviteEnabled");
            return invite.Value == "True";
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}