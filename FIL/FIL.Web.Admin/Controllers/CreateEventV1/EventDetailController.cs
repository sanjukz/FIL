using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.Queries.User;
using FIL.Foundation.Senders;
using FIL.MailChimp.Models;
using FIL.Messaging.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Kitms.Feel.ViewModels.CreateEventV1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FIL.Logging;

namespace FIL.Web.Kitms.Feel.Controllers
{
  public class EventDetailController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly ISessionProvider _sessionProvider;
    private readonly IAccountEmailSender _accountEmailSender;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IClientIpProvider _clientIpProvider;
    private readonly MailChimp.IMailChimpProvider _mailChimpProvider;
    private readonly ILogger _logger;
    public EventDetailController(
        ICommandSender commandSender,
        ISessionProvider sessionProvider,
        IAccountEmailSender accountEmailSender,
        IHttpContextAccessor httpContextAccessor,
        IQuerySender querySender,
        IClientIpProvider clientIpProvider,
        MailChimp.IMailChimpProvider mailChimpProvider,
        ILogger logger)
    {
      _commandSender = commandSender;
      _querySender = querySender;
      _sessionProvider = sessionProvider;
      _accountEmailSender = accountEmailSender;
      _httpContextAccessor = httpContextAccessor;
      _clientIpProvider = clientIpProvider;
      _mailChimpProvider = mailChimpProvider;
      _logger = logger;
    }

    [HttpPost]
    [Route("api/save/event-details")]
    public async Task<EventDetailsViewModel> SaveEventDetail([FromBody] EventDetailsViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var session = await _sessionProvider.Get();
          EventDetailsCommandResult eventDetailsCommandResult = await _commandSender.Send<EventDetailsCommand, EventDetailsCommandResult>(new EventDetailsCommand
          {
            EventDetail = model.EventDetail,
            CurrentStep = model.CurrentStep,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC"),
          });
          if (eventDetailsCommandResult.Success)
          {
            if (model.EventDetail.EventId == 0
              && !FIL.Contracts.Utils.Constant.TestEmail.TestEmails.Contains(session.User.Email)
              && !_httpContextAccessor.HttpContext.Request.Host.Host.Contains("localhost")
              && !_httpContextAccessor.HttpContext.Request.Host.Host.Contains("dev")
              )
            {
              Messaging.Models.Emails.Email email = new Messaging.Models.Emails.Email();
              email.To = "corp@feelitlive.com";
              email.From = "FeelitLIVE  <no-reply@feelitLIVE.com>";
              email.TemplateName = "FILCreateEventAlertToCorp";
              email.Variables = new Dictionary<string, object>
              {
                ["eventname"] = eventDetailsCommandResult.EventDetail.Name,
                ["creatorname"] = session.User.FirstName + " " + session.User.LastName,
                ["eventlink"] = "https://admin.feelitlive.com/host-online/" + eventDetailsCommandResult.EventDetail.EventId + "/basics",
              };
              await _accountEmailSender.Send(email);
            }
            // add user to mailChimp contacts
            try
            {
              var query = await _querySender.Send(new UserSearchQuery
              {
                Email = session.User.Email,
                ChannelId = Channels.Feel,
                SignUpMethodId = SignUpMethods.Regular,
              });
              await _mailChimpProvider.AddFILMember(new MCUserModel
              {
                FirstName = session.User.FirstName,
                LastName = session.User.LastName,
                Email = session.User.Email,
                PhoneCode = session.User.PhoneCode,
                PhoneNumber = session.User.PhoneNumber,
                IsCreator = true,
                SignUpType = "Regular"
              }, query.Country);
            }
            catch (Exception e)
            {
              _logger.Log(Logging.Enums.LogCategory.Error, e);
            }
            return new EventDetailsViewModel
            {
              Success = true,
              EventDetail = eventDetailsCommandResult.EventDetail,
              CurrentStep = eventDetailsCommandResult.CurrentStep,
              CompletedStep = eventDetailsCommandResult.CompletedStep
            };
          }
          else
          {
            return new EventDetailsViewModel { };
          }
        }
        catch (Exception e)
        {
          return new EventDetailsViewModel { };
        }
      }
      else
      {
        return new EventDetailsViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/event-details/{eventId}")]
    public async Task<EventDetailsViewModel> GetTickets(long eventId)
    {
      try
      {
        var queryResult = await _querySender.Send(new EventDetailsQuery
        {
          EventId = eventId
        });
        if (queryResult.Success)
        {
          return new EventDetailsViewModel
          {
            Success = true,
            IsValidLink = queryResult.IsValidLink,
            IsDraft = queryResult.IsDraft,
            EventDetail = queryResult.EventDetail
          };
        }
        else
        {
          return new EventDetailsViewModel
          {
            IsValidLink = queryResult.IsValidLink,
            IsDraft = queryResult.IsDraft,
          };
        }
      }
      catch (Exception e)
      {
        return new EventDetailsViewModel { };
      }
    }
  }
}

