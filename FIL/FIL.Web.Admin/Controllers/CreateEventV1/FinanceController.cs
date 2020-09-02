using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using FIL.Web.Kitms.Feel.ViewModels.Inventory;
using FIL.Contracts.Commands.PlaceInventory;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Core.Providers;
using FIL.Messaging.Senders;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Web.Kitms.Feel.ViewModels.CreateEventV1;
using FIL.Contracts.Queries.CreateEventV1;
using System.Linq;
using FIL.Web.Kitms.Feel.ViewModels.Finance;
using FIL.Contracts.Commands.Payment;
using FIL.Contracts.Enums;
using System.Text;
using FIL.Messaging.Models.Emails;
using FIL.Contracts.Commands.FinanceDetails;
using Gma.QrCodeNet.Encoding.DataEncodation;

namespace FIL.Web.Kitms.Feel.Controllers
{
  public class FinanceController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly ISessionProvider _sessionProvider;
    private readonly IAccountEmailSender _accountEmailSender;

    public FinanceController(
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
    [Route("api/save/event/finance")]
    public async Task<EventFinanceViewModel> SaveEventFinance([FromBody] EventFinanceViewModel model)
    {
      try
      {
        var session = await _sessionProvider.Get();
        EventFinanceDetailCommandResult EventData = await _commandSender.Send<EventFinanceDetailCommand, EventFinanceDetailCommandResult>(new EventFinanceDetailCommand
        {
          EventFinanceDetailModel = model.EventFinanceDetailModel,
          EventId = model.EventId,
          CurrentStep = model.CurrentStep,
          ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC")
        });
        if (EventData.Success)
        {
          try
          {
            // await SaveStripeConnect(EventData.EventAltId, "", false);
          }
          catch (Exception e)
          {
          }
          return new EventFinanceViewModel
          {
            Success = true,
            CurrentStep = EventData.CurrentStep,
            CompletedStep = EventData.CompletedStep
          };
        }
        else
        {
          return new EventFinanceViewModel { };
        }
      }
      catch (Exception e)
      {
        return new EventFinanceViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/event-finance/{eventId}")]
    public async Task<EventFinanceViewModel> GetTickets(long eventId)
    {
      try
      {
        var queryResult = await _querySender.Send(new FinanceQuery
        {
          EventId = eventId
        });
        if (queryResult.Success)
        {
          return new EventFinanceViewModel
          {
            Success = true,
            IsDraft = queryResult.IsDraft,
            IsValidLink = queryResult.IsValidLink,
            StripeAccount = queryResult.StripeAccount,
            StripeConnectAccountId = queryResult.StripeConnectAccountId,
            CurrencyType = queryResult.CurrencyType,
            EventFinanceDetailModel = queryResult.EventFinanceDetailModel,
            EventId = queryResult.EventId,
            EventAltId = queryResult.EventAltId,
            IsoAlphaTwoCode = queryResult.IsoAlphaTwoCode
          };
        }
        else
        {
          return new EventFinanceViewModel
          {
            IsDraft = queryResult.IsDraft,
            IsValidLink = queryResult.IsValidLink,
          };
        }
      }
      catch (Exception e)
      {
        return new EventFinanceViewModel { };
      }
    }
  }
}

