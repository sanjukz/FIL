using System;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Core.Providers;
using FIL.Messaging.Senders;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Web.Kitms.Feel.ViewModels.CreateEventV1;
using FIL.Contracts.Queries.CreateEventV1;

namespace FIL.Web.Kitms.Feel.Controllers
{
  public class TicketController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly ISessionProvider _sessionProvider;
    private readonly IAccountEmailSender _accountEmailSender;

    public TicketController(
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
    [Route("api/save/create-ticket")]
    public async Task<TicketViewModel> SavePlaceCalendar([FromBody] TicketViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var session = await _sessionProvider.Get();
          CreateTicketCommandResult placeInventoryResult = await _commandSender.Send<CreateTicketCommand, CreateTicketCommandResult>(new CreateTicketCommand
          {
            Tickets = model.Tickets,
            EventDetailId = model.EventDetailId,
            EventId = model.EventId,
            IsCreate = model.IsCreate,
            CurrentStep = model.CurrentStep,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC"),
          });
          if (placeInventoryResult.Success)
          {
            return new TicketViewModel
            {
              Success = true,
              EventDetailId = placeInventoryResult.EventDetailId,
              EventId = placeInventoryResult.EventId,
              IsCreate = model.IsCreate,
              Tickets = placeInventoryResult.Tickets,
              CurrentStep = placeInventoryResult.CurrentStep,
              CompletedStep = placeInventoryResult.CompletedStep,
            };
          }
          else
          {
            return new TicketViewModel { };
          }
        }
        catch (Exception e)
        {
          return new TicketViewModel { };
        }
      }
      else
      {
        return new TicketViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/event-tickets/{eventId}")]
    public async Task<TicketViewModel> GetTickets(long eventId, int ticketCategoryTypeId)
    {
      try
      {
        var queryResult = await _querySender.Send(new TicketQuery
        {
          EventId = eventId,
          TicketCategoryTypeId = ticketCategoryTypeId
        });
        if (queryResult.Success)
        {
          return new TicketViewModel
          {
            Success = true,
            IsDraft = queryResult.IsDraft,
            IsValidLink = queryResult.IsValidLink,
            EventDetailId = queryResult.EventDetailId,
            EventId = queryResult.EventId,
            Tickets = queryResult.Tickets
          };
        }
        else
        {
          return new TicketViewModel
          {
            IsDraft = queryResult.IsDraft,
            IsValidLink = queryResult.IsValidLink,
          };
        }
      }
      catch (Exception e)
      {
        return new TicketViewModel { };
      }
    }

    [HttpPost]
    [Route("api/delete/event-ticket")]
    public async Task<DeleteTicketViewModel> DeleteTicket([FromBody] DeleteTicketViewModel model)
    {
      try
      {
        DeleteTicketCommandResult commandResult = await _commandSender.Send<DeleteTicketCommand, DeleteTicketCommandResult>(new DeleteTicketCommand
        {
          ETDAltId = model.ETDAltId,
          TicketLength = model.TicketLength,
          CurrentStep = model.CurrentStep,
          EventId = model.EventId
        });
        return new DeleteTicketViewModel
        {
          Success = commandResult.Success,
          CurrentStep = commandResult.CurrentStep,
          IsTicketSold = commandResult.IsTicketSold,
          CompletedStep = commandResult.CompletedStep
        };
      }
      catch (Exception e)
      {
        return new DeleteTicketViewModel { };
      }
    }
  }
}
