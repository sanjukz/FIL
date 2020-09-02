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
  public class SponsorController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly ISessionProvider _sessionProvider;

    public SponsorController(
        ICommandSender commandSender,
        ISessionProvider sessionProvider,
        IQuerySender querySender)
    {
      _commandSender = commandSender;
      _querySender = querySender;
      _sessionProvider = sessionProvider;
    }

    [HttpPost]
    [Route("api/save/event-sponsor")]
    public async Task<SponsorViewModel> SaveEventSponsor([FromBody] SponsorViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var session = await _sessionProvider.Get();
          SaveSponsorCommandResult saveSponsorCommandResult = await _commandSender.Send<SaveSponsorCommand, SaveSponsorCommandResult>(new SaveSponsorCommand
          {
            SponsorDetail = model.SponsorDetails,
            EventId = model.EventId,
            CurrentStep = model.CurrentStep,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC")
          });
          if (saveSponsorCommandResult.Success)
          {
            return new SponsorViewModel
            {
              Success = true,
              SponsorDetails = saveSponsorCommandResult.SponsorDetail,
              CurrentStep = saveSponsorCommandResult.CurrentStep,
              CompletedStep = saveSponsorCommandResult.CompletedStep
            };
          }
          else
          {
            return new SponsorViewModel { };
          }
        }
        catch (Exception e)
        {
          return new SponsorViewModel { };
        }
      }
      else
      {
        return new SponsorViewModel { };
      }
    }
    [HttpGet]
    [Route("api/get/event-sponsors/{eventId}")]
    public async Task<SponsorViewModel> GetSponsors(long eventId)
    {
      try
      {
        var queryResult = await _querySender.Send(new SponsorQuery
        {
          EventId = eventId
        });
        if (queryResult.Success)
        {
          return new SponsorViewModel
          {
            Success = true,
            EventId = queryResult.EventId,
            IsDraft = queryResult.IsDraft,
            IsValidLink = queryResult.IsValidLink,
            SponsorDetails = queryResult.SponsorDetails
          };
        }
        else
        {
          return new SponsorViewModel
          {
            IsValidLink = queryResult.IsValidLink,
            IsDraft = queryResult.IsDraft,
          };
        }
      }
      catch (Exception e)
      {
        return new SponsorViewModel { };
      }
    }

    [HttpDelete]
    [Route("api/delete/event-sponsor/{sponsorAltId}")]
    public async Task<DeleteSponsorViewModel> DeleteSponsor(Guid sponsorAltId, short currentStep, short completedStep)
    {
      try
      {
        DeleteSponsorCommandResult commandResult = await _commandSender.Send<DeleteSponsorCommand, DeleteSponsorCommandResult>(new DeleteSponsorCommand
        {
          SponsorAltId = sponsorAltId,
          completedStep = completedStep,
          CurrentStep = currentStep
        });
        return new DeleteSponsorViewModel
        {
          Success = commandResult.Success,
          CurrentStep = commandResult.CurrentStep,
          CompletedStep = commandResult.CompletedStep,
        };

      }
      catch (Exception e)
      {
        return new DeleteSponsorViewModel { };
      }
    }

  }
}

