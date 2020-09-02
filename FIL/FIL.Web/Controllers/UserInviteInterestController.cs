using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Configuration;
using FIL.Contracts.Commands._usersWebsiteInvite_Interest;
using FIL.Foundation.Senders;
using FIL.Messaging.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Core.UrlsProvider;
using FIL.Web.Feel.ViewModels.InviteInterest;
using Microsoft.AspNetCore.Mvc;
using FIL.Messaging.Models.Emails;
namespace FIL.Web.Feel.Controllers
{
    public class UserInviteInterestController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ISessionProvider _sessionProvider;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly ISettings _settings;
        private readonly ISiteUrlsProvider _siteUrlsProvider;
        private readonly IAccountEmailSender _emailSender;
        public UserInviteInterestController(ICommandSender commandSender, IQuerySender querySender, ISessionProvider sessionProvider,
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
        [Route("api/inviteinterest/save")]
        public async Task<InviteInterestResponseViewModel> SaveInviteInterest([FromBody]InviteInterestRequestViewModel request)
        {
            if (ModelState.IsValid)
            {
                var session = await _sessionProvider.Get();
                var result = new { Success = true };
                try
                {
                    var userinviteinterest = new UsersWebsiteInvite_InterestCommand()
                    {
                        Email = request.Email,
                        Nationality = request.Nationality,
                        FirstName = request.FirstName,
                        LastName = request.LastName

                    };
                    await _commandSender.Send(userinviteinterest);
                    sendEmail(userinviteinterest);
                    return new InviteInterestResponseViewModel
                    {
                        Success = result.Success,
                    };
                }
                catch (Exception ex)
                {
                    return new InviteInterestResponseViewModel
                    {
                        Success = false
                    };
                }
            }
            else
            {
                return new InviteInterestResponseViewModel
                {
                    Success = false
                };
            }
        }

        private void sendEmail(UsersWebsiteInvite_InterestCommand model)
        {
            try
            {
                string siteUrls = _siteUrlsProvider.GetSiteUrl(_siteIdProvider.GetSiteId());
                Email email = new Email();
                email.To = model.Email;
                email.Bcc = "accounts@kyazoonga.com";
                email.From = "support@feelitLIVE.com";
                email.TemplateName = "FeelInviteInterestRegistration";
                email.Variables = new Dictionary<string, object>
                {
                    ["useremail"] = model.Email,
                    ["websiteurl"] = "<a href='" + siteUrls + "/signup" + "' style='margin-right:100px; '>" + "<h3>Invite Interest</h3></a>",
                    ["subject"] = "Welcome to feelitLIVE! Activate your account to get started.",                    
                };
                _emailSender.Send(email);
            }
            catch (Exception ex)
            {

            }
        }

        

        
    }
}