using System;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Core.Providers;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Web.Kitms.Feel.ViewModels.CreateEventV1;
using FIL.Contracts.Queries.CreateEventV1;

namespace FIL.Web.Kitms.Feel.Controllers
{
  public class ImageController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly ISessionProvider _sessionProvider;

    public ImageController(
        ICommandSender commandSender,
        ISessionProvider sessionProvider,
        IQuerySender querySender)
    {
      _commandSender = commandSender;
      _querySender = querySender;
      _sessionProvider = sessionProvider;
    }

    [HttpPost]
    [Route("api/save/image")]
    public async Task<EventImageViewModel> SaveEventImage([FromBody] EventImageViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var session = await _sessionProvider.Get();
          EventImageCommandResult queryResult = await _commandSender.Send<EventImageCommand, EventImageCommandResult>(new EventImageCommand
          {
            EventImageModel = model.EventImageModel,
            CurrentStep = model.CurrentStep,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC"),
          });
          return new EventImageViewModel
          {
            Success = queryResult.Success,
            EventImageModel = queryResult.EventImageModel,
            IsDraft = queryResult.IsDraft,
            IsValidLink = true,
            CurrentStep = queryResult.CurrentStep,
            CompletedStep = queryResult.CompletedStep,
          };
        }
        catch (Exception e)
        {
          return new EventImageViewModel { };
        }
      }
      else
      {
        return new EventImageViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/event-images/{eventId}")]
    public async Task<EventImageViewModel> GetImageModel(long eventId)
    {
      try
      {
        var queryResult = await _querySender.Send(new EventImageQuery
        {
          EventId = eventId
        });
        return new EventImageViewModel
        {
          Success = queryResult.Success,
          IsDraft = queryResult.IsDraft,
          IsValidLink = queryResult.IsValidLink,
          EventImageModel = queryResult.EventImageModel
        };
      }
      catch (Exception e)
      {
        return new EventImageViewModel { };
      }
    }
  }
}
