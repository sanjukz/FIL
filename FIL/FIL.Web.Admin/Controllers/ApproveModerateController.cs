using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIL.Contracts.Commands.ApproveModeratePlace;
using FIL.Contracts.Commands.Payment;
using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.FeelAdminPlaces;
using FIL.Foundation.Senders;
using FIL.Messaging.Models.Emails;
using FIL.Messaging.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Core.UrlsProvider;
using FIL.Web.Kitms.Feel.ViewModels.ApproveModarate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Kitms.Feel.Controllers
{
  public class ApproveModerateController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly IAccountEmailSender _accountEmailSender;
    private readonly ISiteUrlsProvider _siteUrlsProvider;
    private readonly ISiteIdProvider _siteIdProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISessionProvider _sessionProvider;

    public ApproveModerateController(
        ICommandSender commandSender,
        IAccountEmailSender accountEmailSender,
        ISiteUrlsProvider siteUrlsProvider,
        ISiteIdProvider siteIdProvider,
        IQuerySender querySender,
        ISessionProvider sessionProvider,
        IHttpContextAccessor httpContextAccessor)
    {
      _commandSender = commandSender;
      _querySender = querySender;
      _siteIdProvider = siteIdProvider;
      _accountEmailSender = accountEmailSender;
      _siteUrlsProvider = siteUrlsProvider;
      _httpContextAccessor = httpContextAccessor;
      _sessionProvider = sessionProvider;
    }

    [HttpGet]
    [Route("api/feeladmin/place/{isDeactivatedPlace}")]
    public async Task<ApproveModarateResponseViewModel> Get(bool isDeactivatedPlace)
    {
      var queryResult = await _querySender.Send(new FeelAdminPlacesQuery { IsMyFeel = false, IsDeactivateFeels = isDeactivatedPlace });
      return new ApproveModarateResponseViewModel
      {
        Events = queryResult.Events,
        Users = queryResult.Users,
        MyFeelDetails = queryResult.MyFeelDetails,
        EventAttributes = queryResult.EventAttributes
      };
    }

    [HttpGet]
    [Route("api/approvePlace/{placeAltID}/{isDeactivate}")]
    public async Task<ApproveModaratePlaceResponseViewModel> ApprovePlace(Guid placeAltID,
      bool isDeactivate,
      long eventId,
      FIL.Contracts.Enums.EventStatus eventStatus)
    {
      try
      {
        string _host = _httpContextAccessor.HttpContext.Request.Host.Host;
        var session = await _sessionProvider.Get();
        ApproveModeratePlaceCommandResult placeCalendarResult = await _commandSender.Send<ApproveModeratePlaceCommand, ApproveModeratePlaceCommandResult>(new ApproveModeratePlaceCommand
        {
          PlaceAltId = placeAltID,
          IsDeactivateFeels = isDeactivate,
          EventStatus = eventStatus,
          EventId = eventId,
          UpdateAlgolia = (_host.Contains("dev") || _host.Contains("localhost")) ? false : true
        });
        if (placeCalendarResult.MasterEventTypeId == Contracts.Enums.MasterEventType.Online && !isDeactivate && eventStatus == 0)
        {
          var url = _siteUrlsProvider.GetSiteUrl(_siteIdProvider.GetSiteId());
          Email email = new Email();
          email.To = placeCalendarResult.Email;
          email.From = "FeelitLIVE  <no-reply@feelitLIVE.com>";
          email.TemplateName = "creatoreventapprove";
          email.Variables = new Dictionary<string, object>
          {
            ["eventimage"] = "<img width='100%' alt='Event banner' src='https://s3-us-west-2.amazonaws.com/feelaplace-cdn/images/places/about/" + placeAltID.ToString().ToUpper() + "-about.jpg' />",
            ["eventlink"] = !placeCalendarResult.IsTokanize ? $"{url}/place/{placeCalendarResult.ParentCategorySlug}/{placeCalendarResult.Slug}/{placeCalendarResult.SubCategorySlug}"
            : $"{url}/event/{placeCalendarResult.Slug}/{placeCalendarResult.PlaceAltId.ToString().ToLower()}"

          };
          await _accountEmailSender.Send(email);
        }

        if (placeCalendarResult.MasterEventTypeId == Contracts.Enums.MasterEventType.Online
          && eventStatus == EventStatus.WaitingForApproval
          && placeCalendarResult.Success
          && !FIL.Contracts.Utils.Constant.TestEmail.TestEmails.Contains(session.User.Email)
              && !_host.Contains("localhost")
              && !_host.Contains("dev")
          )
        {
          StripeConnectAccountCommandResult EventData = await _commandSender.Send<StripeConnectAccountCommand, StripeConnectAccountCommandResult>(new StripeConnectAccountCommand
          {
            AuthorizationCode = "",
            ExtraCommisionPercentage = 25,
            ExtraCommisionFlat = 0,
            channels = Channels.Feel,
            EventId = placeCalendarResult.PlaceAltId,
            IsStripeConnect = false,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC")
          });
          List<EmailTemplate> emailTemplates = new List<EmailTemplate>();
          emailTemplates.Add(new EmailTemplate
          {
            TemplateName = "fapcreateevent",
            To = EventData.Email
          });
          emailTemplates.Add(new EmailTemplate
          {
            TemplateName = "FILSubmitEventForApprovalAlertToCorp",
            To = "corp@feelitlive.com"
          });
          foreach (var emailTemplate in emailTemplates)
          {
            StringBuilder ticketCategories = new StringBuilder();
            foreach (var eventTicketDetail in EventData.EventTicketDetail)
            {
              var eventTicketAttribute = EventData.EventTicketAttribute.Where(s => s.EventTicketDetailId == eventTicketDetail.Id).FirstOrDefault();
              var ticketCategory = EventData.TicketCategories.Where(s => s.Id == eventTicketDetail.TicketCategoryId).FirstOrDefault();
              ticketCategories.Append("<tr>");
              ticketCategories.Append($"<td style='font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 14px; line-height: 26px; color: #333;' width='200'>{ticketCategory.Name}: </td> <td style='font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 14px; line-height: 26px; color: #333;'>{EventData.CurrencyType.Code} {eventTicketAttribute.Price}</td> ");
              ticketCategories.Append("</tr>");
            }
            Email email = new Email();
            email.To = emailTemplate.To;
            email.From = "FeelitLIVE  <no-reply@feelitLIVE.com>";
            email.TemplateName = emailTemplate.TemplateName;
            email.Variables = new Dictionary<string, object>
            {
              ["eventimage"] = "<img width='100%' alt='Event banner' src='https://s3-us-west-2.amazonaws.com/feelaplace-cdn/images/places/about/" + EventData.Event.AltId.ToString().ToUpper() + "-about.jpg' />",
              ["eventname"] = EventData.Event.Name,
              ["eventdatetime"] = $"{EventData.EventDetail.StartDateTime.ToString("MMM dd, yyyy").ToUpper()} {EventData.DayTimeMappings.StartTime}",
              ["ticketcategories"] = ticketCategories.ToString(),
            };
            await _accountEmailSender.Send(email);
          }
        }
        return new ApproveModaratePlaceResponseViewModel { Success = true };
      }
      catch (Exception e)
      {
        return new ApproveModaratePlaceResponseViewModel { Success = false };
      }
    }
  }
  public class EmailTemplate
  {
    public string To { get; set; }
    public string TemplateName { get; set; }
  }
}
