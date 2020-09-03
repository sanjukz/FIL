using System;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Core.Providers;
using FIL.Messaging.Senders;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Web.Admin.ViewModels.CreateEventV1;
using FIL.Contracts.Queries.CreateEventV1;

namespace FIL.Web.Admin.Controllers
{
  public class EventPerformanceController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly ISessionProvider _sessionProvider;
    private readonly IAccountEmailSender _accountEmailSender;

    public EventPerformanceController(
        ICommandSender commandSender,
        ISessionProvider sessionProvider,
        IAccountEmailSender accountEmailSender,
        IQuerySender querySender)
    {
      _commandSender = commandSender;
      _querySender = querySender;
      _sessionProvider = sessionProvider;
      _accountEmailSender = accountEmailSender;
    }

    [HttpPost]
    [Route("api/save/event-performance")]
    public async Task<EventPerformanceViewModel> SaveEventPerformace([FromBody] EventPerformanceViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var session = await _sessionProvider.Get();
          EventPerformanceCommandResult eventPerformanceCommand = await _commandSender.Send<EventPerformanceCommand, EventPerformanceCommandResult>(new EventPerformanceCommand
          {

            EventId = model.EventId,
            CurrentStep = model.CurrentStep,
            PerformanceTypeModel = model.PerformanceTypeModel,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC"),
          });
          return new EventPerformanceViewModel
          {
            Success = eventPerformanceCommand.Success,
            EventAltId = eventPerformanceCommand.EventAltId,
            EventId = eventPerformanceCommand.EventId,
            OnlineEventType = eventPerformanceCommand.OnlineEventType,
            PerformanceTypeModel = eventPerformanceCommand.PerformanceTypeModel,
            CurrentStep = eventPerformanceCommand.CurrentStep,
            CompletedStep = eventPerformanceCommand.CompletedStep
          };
        }
        catch (Exception e)
        {
          return new EventPerformanceViewModel { };
        }
      }
      else
      {
        return new EventPerformanceViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/event-performance/{eventId}")]
    public async Task<EventPerformanceViewModel> GetTickets(long eventId)
    {
      try
      {
        var queryResult = await _querySender.Send(new EventPerformanceQuery
        {
          EventId = eventId
        });
        return new EventPerformanceViewModel
        {
          Success = queryResult.Success,
          EventId = queryResult.EventId,
          EventAltId = (System.Guid)queryResult.EventAltId,
          OnlineEventType = queryResult.OnlineEventType,
          EventFrequencyType = queryResult.EventFrequencyType,
          PerformanceTypeModel = queryResult.PerformanceTypeModel
        };
      }
      catch (Exception e)
      {
        return new EventPerformanceViewModel { };
      }
    }
  }
}

