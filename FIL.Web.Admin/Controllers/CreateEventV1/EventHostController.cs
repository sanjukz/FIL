using System;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Core.Providers;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Web.Admin.ViewModels.CreateEventV1;
using FIL.Contracts.Queries.CreateEventV1;

namespace FIL.Web.Admin.Controllers
{
  public class EventHostController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly ISessionProvider _sessionProvider;

    public EventHostController(
        ICommandSender commandSender,
        ISessionProvider sessionProvider,
        IQuerySender querySender)
    {
      _commandSender = commandSender;
      _querySender = querySender;
      _sessionProvider = sessionProvider;
    }

    [HttpPost]
    [Route("api/save/event-hots")]
    public async Task<EventHostsViewModel> SaveEventDetail([FromBody] EventHostsViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var session = await _sessionProvider.Get();
          EventHostCommandResult eventDetailsCommandResult = await _commandSender.Send<EventHostCommand, EventHostCommandResult>(new EventHostCommand
          {
            EventHostMappings = model.EventHostMapping,
            EventId = model.EventId,
            CurrentStep = model.CurrentStep,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC")
          });
          if (eventDetailsCommandResult.Success)
          {
            return new EventHostsViewModel
            {
              Success = true,
              EventHostMapping = eventDetailsCommandResult.EventHostMappings,
              CurrentStep = eventDetailsCommandResult.CurrentStep,
              CompletedStep = eventDetailsCommandResult.CompletedStep
            };
          }
          else
          {
            return new EventHostsViewModel { };
          }
        }
        catch (Exception e)
        {
          return new EventHostsViewModel { };
        }
      }
      else
      {
        return new EventHostsViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/event-hosts/{eventId}")]
    public async Task<EventHostsViewModel> GetTickets(long eventId)
    {
      try
      {
        var queryResult = await _querySender.Send(new EventHostQuery
        {
          EventId = eventId
        });
        if (queryResult.Success)
        {
          return new EventHostsViewModel
          {
            Success = true,
            EventId = queryResult.EventId,
            IsDraft = queryResult.IsDraft,
            IsValidLink = queryResult.IsValidLink,
            EventHostMapping = queryResult.EventHostMapping
          };
        }
        else
        {
          return new EventHostsViewModel
          {
            IsValidLink = queryResult.IsValidLink,
            IsDraft = queryResult.IsDraft,
          };
        }
      }
      catch (Exception e)
      {
        return new EventHostsViewModel { };
      }
    }

    [HttpDelete]
    [Route("api/delete/event-host/{hostAltId}")]
    public async Task<DeleteEventHostViewModel> DeleteHost(Guid hostAltId, short currentStep, short ticketLength)
    {
      try
      {
        DeleteHostCommandResult commandResult = await _commandSender.Send<DeleteHostCommand, DeleteHostCommandResult>(new DeleteHostCommand
        {
          EventHostAltId = hostAltId,
          TicketLength = ticketLength,
          CurrentStep = currentStep
        });
        return new DeleteEventHostViewModel
        {
          Success = commandResult.Success,
          CurrentStep = commandResult.CurrentStep,
          CompletedStep = commandResult.CompletedStep,
          IsHostStreamLinkCreated = commandResult.IsHostStreamLinkCreated
        };
      }
      catch (Exception e)
      {
        return new DeleteEventHostViewModel { };
      }
    }

  }
}

