using System;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Core.Providers;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Web.Kitms.Feel.ViewModels.CreateEventV1;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Web.Kitms.Feel.Providers;

namespace FIL.Web.Kitms.Feel.Controllers
{
  public class EventScheduleController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly ISessionProvider _sessionProvider;
    private readonly IUtcTimeProvider _utcTimeProvider;
    private readonly ILocalTimeProvider _localTimeProvider;

    public EventScheduleController(
        ICommandSender commandSender,
        ISessionProvider sessionProvider,
        IUtcTimeProvider utcTimeProvider,
        ILocalTimeProvider localTimeProvider,
        IQuerySender querySender)
    {
      _commandSender = commandSender;
      _querySender = querySender;
      _sessionProvider = sessionProvider;
      _utcTimeProvider = utcTimeProvider;
      _localTimeProvider = localTimeProvider;
    }

    [HttpPost]
    [Route("api/save/event-schedule")]
    public async Task<EventScheduleViewModel> SaveEventDetail([FromBody] EventScheduleViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var session = await _sessionProvider.Get();
          model.EventScheduleModel.StartDateTime = new DateTime(model.EventScheduleModel.StartDateTime.Year, model.EventScheduleModel.StartDateTime.Month, model.EventScheduleModel.StartDateTime.Day, Convert.ToInt32(model.EventScheduleModel.LocalStartTime.Split(":")[0]), Convert.ToInt32(model.EventScheduleModel.LocalStartTime.Split(":")[1]), 0);
          model.EventScheduleModel.EndDateTime = new DateTime(model.EventScheduleModel.EndDateTime.Year, model.EventScheduleModel.EndDateTime.Month, model.EventScheduleModel.EndDateTime.Day, Convert.ToInt32(model.EventScheduleModel.LocalEndTime.Split(":")[0]), Convert.ToInt32(model.EventScheduleModel.LocalEndTime.Split(":")[1]), 0);
          if (!model.EventScheduleModel.TimeZoneOffset.Contains("-") && !model.EventScheduleModel.TimeZoneOffset.Contains("+"))
          {
            model.EventScheduleModel.TimeZoneOffset = "+" + model.EventScheduleModel.TimeZoneOffset;
          }
          model.EventScheduleModel.StartDateTime = _utcTimeProvider.GetUtcTime(model.EventScheduleModel.StartDateTime, model.EventScheduleModel.TimeZoneOffset);
          model.EventScheduleModel.EndDateTime = _utcTimeProvider.GetUtcTime(model.EventScheduleModel.EndDateTime, model.EventScheduleModel.TimeZoneOffset);
          model.EventScheduleModel.VenueId = 10694;
          EventScheduleCommandResult eventDetailsCommandResult = await _commandSender.Send<EventScheduleCommand, EventScheduleCommandResult>(new EventScheduleCommand
          {
            CurrentStep = model.CurrentStep,
            EventScheduleModel = model.EventScheduleModel,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC"),
          });
          if (eventDetailsCommandResult.Success)
          {
            eventDetailsCommandResult.EventScheduleModel.LocalStartDateTime = _localTimeProvider.GetLocalTime(eventDetailsCommandResult.EventScheduleModel.StartDateTime, eventDetailsCommandResult.EventScheduleModel.TimeZoneOffset);
            eventDetailsCommandResult.EventScheduleModel.LocalEndDateTime = _localTimeProvider.GetLocalTime(eventDetailsCommandResult.EventScheduleModel.EndDateTime, eventDetailsCommandResult.EventScheduleModel.TimeZoneOffset);
            return new EventScheduleViewModel
            {
              Success = true,
              CurrentStep = eventDetailsCommandResult.CurrentStep,
              CompletedStep = eventDetailsCommandResult.CompletedStep,
              EventScheduleModel = eventDetailsCommandResult.EventScheduleModel
            };
          }
          else
          {
            return new EventScheduleViewModel { };
          }
        }
        catch (Exception e)
        {
          return new EventScheduleViewModel { };
        }
      }
      else
      {
        return new EventScheduleViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/event-schedule/{eventId}")]
    public async Task<EventScheduleViewModel> GetTickets(long eventId)
    {
      try
      {
        var queryResult = await _querySender.Send(new EventScheduleQuery
        {
          EventId = eventId
        });
        if (queryResult.Success)
        {
          if (queryResult.IsValidLink && !queryResult.IsDraft)
          {
            queryResult.EventScheduleModel.LocalStartDateTime = _localTimeProvider.GetLocalTime(queryResult.EventScheduleModel.StartDateTime, queryResult.EventScheduleModel.TimeZoneOffset);
            queryResult.EventScheduleModel.LocalEndDateTime = _localTimeProvider.GetLocalTime(queryResult.EventScheduleModel.EndDateTime, queryResult.EventScheduleModel.TimeZoneOffset);
          }
          return new EventScheduleViewModel
          {
            Success = true,
            IsDraft = queryResult.IsDraft,
            IsValidLink = queryResult.IsValidLink,
            EventScheduleModel = queryResult.EventScheduleModel
          };
        }
        else
        {
          return new EventScheduleViewModel
          {
            IsDraft = queryResult.IsDraft,
            IsValidLink = queryResult.IsValidLink
          };
        }
      }
      catch (Exception e)
      {
        return new EventScheduleViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/recurrance-schedule")]
    public async Task<EventRecurranceResponseViewModel> GetRecurranceSchedule(long eventId, DateTime startDate, DateTime endDate)
    {
      try
      {
        var queryResult = await _querySender.Send(new EventRecurranceScheduleQuery
        {
          EventId = eventId,
          StartDate = startDate,
          EndDate = endDate
        });
        if (queryResult.Success)
        {
          return new EventRecurranceResponseViewModel
          {
            Success = true,
            IsDraft = queryResult.IsDraft,
            IsValidLink = queryResult.IsValidLink,
            EventRecurranceScheduleModel = queryResult.EventRecurranceScheduleModel
          };
        }
        else
        {
          return new EventRecurranceResponseViewModel
          {
            IsDraft = queryResult.IsDraft,
            IsValidLink = queryResult.IsValidLink
          };
        }
      }
      catch (Exception e)
      {
        return new EventRecurranceResponseViewModel { };
      }
    }

    [HttpPost]
    [Route("api/schedule/bulk-insert")]
    public async Task<EventScheduleViewModel> SaveSchedule([FromBody] EventRecurranceViewModel eventRecurranceViewModel)
    {
      try
      {
        var session = await _sessionProvider.Get();
        if (eventRecurranceViewModel.EventDetailId == 0)
        {
          EventScheduleCommandResult eventDetailsCommandResult1 = await _commandSender.Send<EventScheduleCommand, EventScheduleCommandResult>(new EventScheduleCommand
          {
            CurrentStep = eventRecurranceViewModel.CurrentStep,
            EventFrequencyType = Contracts.Enums.EventFrequencyType.Recurring,
            EventScheduleModel = new Contracts.Models.CreateEventV1.EventScheduleModel
            {
              EventId = eventRecurranceViewModel.EventId,
              StartDateTime = eventRecurranceViewModel.StartDateTime,
              EndDateTime = eventRecurranceViewModel.EndDateTime,
              EventFrequencyType = Contracts.Enums.EventFrequencyType.Recurring,
              IsEnabled = true,
              LocalStartTime = "00:00",
              LocalEndTime = "00:00",
              TimeZoneAbbrivation = eventRecurranceViewModel.TimeZoneAbbrivation,
              TimeZoneOffset = eventRecurranceViewModel.TimeZoneOffset,
              VenueId = 1094
            },
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC"),
          });
          if (!eventDetailsCommandResult1.Success)
          {
            return new EventScheduleViewModel { };
          }
        }
        var eventDetailsCommandResult = await _commandSender.Send<EventRecurranceCommand, EventRecurranceCommandResult>(new EventRecurranceCommand
        {
          ActionType = ActionType.BulkInsert,
          EventScheduleId = eventRecurranceViewModel.EventScheduleId,
          ScheduleDetailId = eventRecurranceViewModel.ScheduleDetailId,
          StartDateTime = eventRecurranceViewModel.StartDateTime,
          EndDateTime = eventRecurranceViewModel.EndDateTime,
          DayIds = eventRecurranceViewModel.DayIds,
          EventId = eventRecurranceViewModel.EventId,
          LocalStartTime = eventRecurranceViewModel.LocalStartTime,
          LocalEndTime = eventRecurranceViewModel.LocalEndTime,
          OccuranceCount = eventRecurranceViewModel.OccuranceCount,
          OccuranceType = eventRecurranceViewModel.OccuranceType,
          TimeZoneOffSet = eventRecurranceViewModel.TimeZoneOffset,
          EventFrequencyType = eventRecurranceViewModel.EventFrequencyType,
          TimeZoneAbbrivation = eventRecurranceViewModel.TimeZoneAbbrivation,
          ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC")
        });
        if (eventDetailsCommandResult.Success)
        {
          return new EventScheduleViewModel
          {
            Success = true
          };
        }
        else
        {
          return new EventScheduleViewModel { };
        }
      }
      catch (Exception e)
      {
        return new EventScheduleViewModel { };
      }
    }

    [HttpPost]
    [Route("api/reschedule/bulk-reschedule")]
    public async Task<EventScheduleViewModel> SaveBulkReschedule([FromBody] EventRecurranceViewModel eventRecurranceViewModel)
    {
      try
      {
        var session = await _sessionProvider.Get();
        var eventDetailsCommandResult = await _commandSender.Send<EventRecurranceCommand, EventRecurranceCommandResult>(new EventRecurranceCommand
        {
          ActionType = ActionType.BulkReschedule,
          EventScheduleId = eventRecurranceViewModel.EventScheduleId,
          ScheduleDetailId = eventRecurranceViewModel.ScheduleDetailId,
          StartDateTime = eventRecurranceViewModel.StartDateTime,
          EndDateTime = eventRecurranceViewModel.EndDateTime,
          DayIds = eventRecurranceViewModel.DayIds,
          EventId = eventRecurranceViewModel.EventId,
          LocalStartTime = eventRecurranceViewModel.LocalStartTime,
          LocalEndTime = eventRecurranceViewModel.LocalEndTime,
          OccuranceCount = eventRecurranceViewModel.OccuranceCount,
          OccuranceType = eventRecurranceViewModel.OccuranceType,
          TimeZoneOffSet = eventRecurranceViewModel.TimeZoneOffset,
          EventFrequencyType = eventRecurranceViewModel.EventFrequencyType,
          ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC")
        });
        if (eventDetailsCommandResult.Success)
        {
          return new EventScheduleViewModel
          {
            Success = true
          };
        }
        else
        {
          return new EventScheduleViewModel { };
        }
      }
      catch (Exception e)
      {
        return new EventScheduleViewModel { };
      }
    }

    [HttpPost]
    [Route("api/reschedule/single")]
    public async Task<EventScheduleViewModel> SaveSingleReschedule([FromBody] EventRecurranceViewModel eventRecurranceViewModel)
    {
      try
      {
        var session = await _sessionProvider.Get();
        var eventDetailsCommandResult = await _commandSender.Send<EventRecurranceCommand, EventRecurranceCommandResult>(new EventRecurranceCommand
        {
          ActionType = ActionType.SingleReschedule,
          EventScheduleId = eventRecurranceViewModel.EventScheduleId,
          ScheduleDetailId = eventRecurranceViewModel.ScheduleDetailId,
          StartDateTime = eventRecurranceViewModel.StartDateTime,
          EndDateTime = eventRecurranceViewModel.EndDateTime,
          DayIds = eventRecurranceViewModel.DayIds,
          EventId = eventRecurranceViewModel.EventId,
          LocalStartTime = eventRecurranceViewModel.LocalStartTime,
          LocalEndTime = eventRecurranceViewModel.LocalEndTime,
          OccuranceCount = eventRecurranceViewModel.OccuranceCount,
          OccuranceType = eventRecurranceViewModel.OccuranceType,
          TimeZoneOffSet = eventRecurranceViewModel.TimeZoneOffset,
          EventFrequencyType = eventRecurranceViewModel.EventFrequencyType,
          ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC")
        });
        if (eventDetailsCommandResult.Success)
        {
          return new EventScheduleViewModel
          {
            Success = true
          };
        }
        else
        {
          return new EventScheduleViewModel { };
        }
      }
      catch (Exception e)
      {
        return new EventScheduleViewModel { };
      }
    }

    [HttpPost]
    [Route("api/schedule/bulk-delete")]
    public async Task<EventScheduleViewModel> DeleteBulkSchedule([FromBody] EventRecurranceViewModel eventRecurranceViewModel)
    {
      try
      {
        var session = await _sessionProvider.Get();
        var eventDetailsCommandResult = await _commandSender.Send<EventRecurranceCommand, EventRecurranceCommandResult>(new EventRecurranceCommand
        {
          ActionType = ActionType.BulkDelete,
          EventScheduleId = eventRecurranceViewModel.EventScheduleId,
          ScheduleDetailId = eventRecurranceViewModel.ScheduleDetailId,
          StartDateTime = eventRecurranceViewModel.StartDateTime,
          EndDateTime = eventRecurranceViewModel.EndDateTime,
          DayIds = eventRecurranceViewModel.DayIds,
          EventId = eventRecurranceViewModel.EventId,
          LocalStartTime = eventRecurranceViewModel.LocalStartTime,
          LocalEndTime = eventRecurranceViewModel.LocalEndTime,
          OccuranceCount = eventRecurranceViewModel.OccuranceCount,
          OccuranceType = eventRecurranceViewModel.OccuranceType,
          TimeZoneOffSet = eventRecurranceViewModel.TimeZoneOffset,
          EventFrequencyType = eventRecurranceViewModel.EventFrequencyType,
          ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC")
        });
        if (eventDetailsCommandResult.Success)
        {
          return new EventScheduleViewModel
          {
            Success = true
          };
        }
        else
        {
          return new EventScheduleViewModel { };
        }
      }
      catch (Exception e)
      {
        return new EventScheduleViewModel { };
      }
    }

    [HttpPost]
    [Route("api/schedule/delete")]
    public async Task<EventScheduleViewModel> DeleteSingleSchedule([FromBody] EventRecurranceViewModel eventRecurranceViewModel)
    {
      try
      {
        var session = await _sessionProvider.Get();
        var eventDetailsCommandResult = await _commandSender.Send<EventRecurranceCommand, EventRecurranceCommandResult>(new EventRecurranceCommand
        {
          ActionType = ActionType.SingleDelete,
          EventScheduleId = eventRecurranceViewModel.EventScheduleId,
          ScheduleDetailId = eventRecurranceViewModel.ScheduleDetailId,
          StartDateTime = eventRecurranceViewModel.StartDateTime,
          EndDateTime = eventRecurranceViewModel.EndDateTime,
          DayIds = eventRecurranceViewModel.DayIds,
          EventId = eventRecurranceViewModel.EventId,
          LocalStartTime = eventRecurranceViewModel.LocalStartTime,
          LocalEndTime = eventRecurranceViewModel.LocalEndTime,
          OccuranceCount = eventRecurranceViewModel.OccuranceCount,
          OccuranceType = eventRecurranceViewModel.OccuranceType,
          TimeZoneOffSet = eventRecurranceViewModel.TimeZoneOffset,
          EventFrequencyType = eventRecurranceViewModel.EventFrequencyType,
          ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC")
        });
        if (eventDetailsCommandResult.Success)
        {
          return new EventScheduleViewModel
          {
            Success = true
          };
        }
        else
        {
          return new EventScheduleViewModel { };
        }
      }
      catch (Exception e)
      {
        return new EventScheduleViewModel { };
      }
    }

  }
}

