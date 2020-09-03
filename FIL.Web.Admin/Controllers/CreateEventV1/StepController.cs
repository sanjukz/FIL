using System;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Core.Providers;
using FIL.Web.Admin.ViewModels.CreateEventV1;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.Commands.CreateEventV1;

namespace FIL.Web.Admin.Controllers
{
  public class StepController : Controller
  {
    private readonly IQuerySender _querySender;
    private readonly ICommandSender _commandSender;
    private readonly ISessionProvider _sessionProvider;

    public StepController(
        ISessionProvider sessionProvider,
        ICommandSender commandSender,
        IQuerySender querySender)
    {
      _querySender = querySender;
      _sessionProvider = sessionProvider;
      _commandSender = commandSender;
    }

    [HttpPost]
    [Route("api/save/current-step")]
    public async Task<StepViewModel> SavePlaceCalendar([FromBody] StepViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var session = await _sessionProvider.Get();
          EventStepCommandResult eventStepCommandResult = await _commandSender.Send<EventStepCommand, EventStepCommandResult>(new EventStepCommand
          {

            EventId = model.EventId,
            CurrentStep = model.CurrentStep,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC"),
          });
          return new StepViewModel
          {
            CompletedStep = eventStepCommandResult.CompletedStep,
            CurrentStep = eventStepCommandResult.CurrentStep,
            EventId = model.EventId,
            Success = eventStepCommandResult.Success
          };
        }
        catch (Exception e)
        {
          return new StepViewModel { };
        }
      }
      else
      {
        return new StepViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/steps/{masterEvent}")]
    public async Task<StepViewModel> GetTickets(FIL.Contracts.Enums.MasterEventType masterEvent, int eventId)
    {
      try
      {
        var queryResult = await _querySender.Send(new StepQuery
        {
          EventId = eventId,
          MasterEventType = masterEvent
        });
        return new StepViewModel
        {
          CompletedStep = queryResult.CompletedStep,
          CurrentStep = queryResult.CurrentStep,
          EventId = queryResult.EventId,
          StepModel = queryResult.StepModel,
          EventName = queryResult.EventName,
          EventStatus = queryResult.EventStatus,
          IsTransacted = queryResult.IsTransacted,
          EventFrequencyType = queryResult.EventFrequencyType,
          Success = queryResult.Success
        };
      }
      catch (Exception e)
      {
        return new StepViewModel { };
      }
    }
  }
}

