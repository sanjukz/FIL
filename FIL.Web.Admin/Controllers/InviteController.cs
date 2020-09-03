using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Invite;
using FIL.Contracts.Queries;
using FIL.Contracts.Queries.WebsiteInvite;
using FIL.Foundation.Senders;
using FIL.Messaging.Models.Emails;
using FIL.Messaging.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Core.UrlsProvider;
using FIL.Web.Admin.ViewModels.Invite;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Admin.Controllers
{
    public class InviteController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ISiteUrlsProvider _siteUrlsProvider;
        private readonly IAccountEmailSender _emailSender;
        private readonly ISiteIdProvider _siteIdProvider;

        public InviteController(ICommandSender commandSender, IQuerySender querySender, ISiteUrlsProvider siteUrlsProvider,
            IAccountEmailSender emailSender, ISiteIdProvider siteIdProvider)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _siteUrlsProvider = siteUrlsProvider;
            _emailSender = emailSender;
            _siteIdProvider = siteIdProvider;
        }

        [HttpGet]
        [Route("api/invite/all")]
        public async Task<InviteResponseViewModel> GetAll()
        {
            var queryResult = await _querySender.Send(new SearchUserWebsiteInviteQuery() { IsUsed = false });

            return queryResult.UsersWebsiteInvite != null ? new InviteResponseViewModel { Invites = queryResult.UsersWebsiteInvite } : null;
        }


        [HttpGet]
        [Route("api/invitesearch/{searchstring}")]
        public async Task<InviteResponseViewModel> SearchInviteData(string searchstring)
        {
            var values = searchstring.Split(',');
            var queryResult = await _querySender.Send(new SearchUserWebsiteInviteQuery() { SearchString = values[0], IsUsed = values[1] == "true" });
            return queryResult.UsersWebsiteInvite != null ? new InviteResponseViewModel { Invites = queryResult.UsersWebsiteInvite } : null;
        }

        [HttpGet]
        [Route("api/invitesummary")]
        public async Task<InviteSummaryViewModal> InviteSummary()

        {
            var queryResult = await _querySender.Send(new InviteSummaryQuery() { ISUsed = false });
            InviteSummaryViewModal obj = new InviteSummaryViewModal();
            obj.TotalMails = queryResult.TotalMails;
            obj.UnUsedMails = queryResult.UnUsedMails;
            obj.UsedMails = queryResult.UsedMails;
            return obj;


        }

        [HttpPost]
        [Route("api/invite/edit")]
        public async Task<IActionResult> EditInvite([FromBody] UpdateInviteRequestViewModel inviteVM)
        {
            var result = new { Succeeded = true };
            await _commandSender.Send(new UpdateInviteCommand()
            {
                UserEmail = inviteVM.Email,
                UserInviteCode = inviteVM.InviteCode,
                IsUsed = inviteVM.IsUsed,
                Id = inviteVM.Id
            });
            if (result.Succeeded)
            {
                return Ok(true);
            }
            else
            {

                return BadRequest(false);
            }
        }


        [HttpPost]
        [Route("api/invite/sendemail")]
        public async Task<IActionResult> SendInviteEmail([FromBody] UpdateInviteRequestViewModel updateVM)
        {
            bool flag = sendEmail(new UserInviteCommand() { UserEmail = updateVM.Email, UserInviteCode = updateVM.InviteCode });
            if (flag)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(false);
            }
        }

        private bool sendEmail(UserInviteCommand model)
        {
            try
            {
                string siteUrls = _siteUrlsProvider.GetSiteUrl(Contracts.Enums.Site.feelaplaceSite);
                Email email = new Email();
                email.To = model.UserEmail;
                email.Bcc = "accounts@feelitlive.com";
                email.From = "FeelitLIVE  <no-reply@feelitLIVE.com>";
                email.TemplateName = "FeelInviteeRegistration";

                email.Variables = new Dictionary<string, object>
                {
                    ["signinlink"] = "<a href='" + siteUrls + "/signup" + "' style='margin-right:100px; '>" + "<h3>feelitLIVE.com</h3></a>",
                    ["userpassword"] = model.UserInviteCode
                };
                _emailSender.Send(email);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}