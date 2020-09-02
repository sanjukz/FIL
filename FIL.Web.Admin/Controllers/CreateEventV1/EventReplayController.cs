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
  public class EventReplayController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly ISessionProvider _sessionProvider;
    private readonly ILogger _logger;

    public EventReplayController(
        ICommandSender commandSender,
        ISessionProvider sessionProvider,
        IQuerySender querySender,
        ILogger logger)
    {
      _commandSender = commandSender;
      _querySender = querySender;
      _sessionProvider = sessionProvider;
      _logger = logger;
    }

    [HttpPost]
    [Route("api/save/replay-details")]
    public async Task<EventReplayViewModel> SaveEventDetail([FromBody] EventReplayViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var session = await _sessionProvider.Get();
          ReplayDetailCommandResult eventDetailsCommandResult = await _commandSender.Send<ReplayDetailCommand, ReplayDetailCommandResult>(new ReplayDetailCommand
          {
            EventId = model.EventId,
            CurrentStep = model.CurrentStep,
            ReplayDetailModel = model.ReplayDetailModel,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC"),
          });
          if (eventDetailsCommandResult.Success)
          {
            return new EventReplayViewModel
            {
              Success = true,
              EventId = model.EventId,
              CompletedStep = eventDetailsCommandResult.CompletedStep,
              CurrentStep = model.CurrentStep,
              ReplayDetailModel = model.ReplayDetailModel
            };
          }
          else
          {
            return new EventReplayViewModel { };
          }
        }
        catch (Exception e)
        {
          return new EventReplayViewModel { };
        }
      }
      else
      {
        return new EventReplayViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/replay-details/{eventId}")]
    public async Task<EventReplayViewModel> GetTickets(long eventId)
    {
      try
      {
        var queryResult = await _querySender.Send(new EventReplayQuery
        {
          EventId = eventId
        });
        if (queryResult.Success)
        {
          return new EventReplayViewModel
          {
            Success = true,
            EventId = queryResult.EventId,
            ReplayDetailModel = queryResult.ReplayDetailModel
          };
        }
        else
        {
          return new EventReplayViewModel { };
        }
      }
      catch (Exception e)
      {
        return new EventReplayViewModel { };
      }
    }
  }
}

