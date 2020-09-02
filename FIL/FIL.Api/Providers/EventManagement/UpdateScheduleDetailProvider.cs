using FIL.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers.EventManagement
{
    public interface IUpdateScheduleDetailProvider
    {
        List<FIL.Contracts.DataModels.ScheduleDetail> GetScheduleDetails(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand);
    }

    public class UpdateScheduleDetailProvider : IUpdateScheduleDetailProvider
    {
        private ICommonUtilityProvider _commonUtilityProvider;
        private IEventScheduleRepository _eventScheduleRepository;
        private IScheduleDetailRepository _scheduleDetailRepository;

        public UpdateScheduleDetailProvider(
            ICommonUtilityProvider commonUtilityProvider,
            IEventScheduleRepository eventScheduleRepository,
            IScheduleDetailRepository scheduleDetailRepository
            )
        {
            _commonUtilityProvider = commonUtilityProvider;
            _eventScheduleRepository = eventScheduleRepository;
            _scheduleDetailRepository = scheduleDetailRepository;
        }

        protected void SaveEventSchedule(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand)
        {
            if (eventRecurranceCommand.ActionType == Contracts.Commands.CreateEventV1.ActionType.BulkReschedule)
            {
                var eventSchedule = _eventScheduleRepository.Get(eventRecurranceCommand.EventScheduleId);
                eventSchedule.StartDateTime = _commonUtilityProvider.GetUtcDate(eventRecurranceCommand.StartDateTime, eventRecurranceCommand.LocalStartTime, eventRecurranceCommand.TimeZoneOffSet);
                eventSchedule.EndDateTime = _commonUtilityProvider.GetUtcDate(eventRecurranceCommand.StartDateTime, eventRecurranceCommand.LocalStartTime, eventRecurranceCommand.TimeZoneOffSet);
                eventSchedule.Name = eventRecurranceCommand.StartDateTime.ToString() + " - " + eventRecurranceCommand.EndDateTime.ToString();
                eventSchedule.ModifiedBy = eventRecurranceCommand.ModifiedBy;
                eventSchedule.UpdatedBy = eventRecurranceCommand.ModifiedBy;
                eventSchedule.UpdatedUtc = DateTime.UtcNow;
                _eventScheduleRepository.Save(eventSchedule);
            }
        }

        public List<FIL.Contracts.DataModels.ScheduleDetail> GetScheduleDetails(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand)
        {
            List<FIL.Contracts.DataModels.ScheduleDetail> eventSchedules = new List<Contracts.DataModels.ScheduleDetail>();
            List<FIL.Contracts.DataModels.ScheduleDetail> eventSchedules1 = new List<Contracts.DataModels.ScheduleDetail>();
            var startDateTime = _commonUtilityProvider.GetUtcDate(eventRecurranceCommand.StartDateTime, eventRecurranceCommand.LocalStartTime, eventRecurranceCommand.TimeZoneOffSet);
            var endDateTime = _commonUtilityProvider.GetUtcDate(eventRecurranceCommand.EndDateTime, eventRecurranceCommand.LocalEndTime, eventRecurranceCommand.TimeZoneOffSet);

            if (eventRecurranceCommand.ActionType == Contracts.Commands.CreateEventV1.ActionType.BulkReschedule)
            {
                SaveEventSchedule(eventRecurranceCommand);
                eventSchedules = _scheduleDetailRepository.GetAllByEventScheduleId(eventRecurranceCommand.EventScheduleId).ToList();
            }
            else
            {
                var scheduleDetail = _scheduleDetailRepository.Get(eventRecurranceCommand.ScheduleDetailId);
                scheduleDetail.StartDateTime = startDateTime;
                scheduleDetail.EndDateTime = endDateTime;
                scheduleDetail.UpdatedBy = eventRecurranceCommand.ModifiedBy;
                scheduleDetail.ModifiedBy = eventRecurranceCommand.ModifiedBy;
                scheduleDetail.UpdatedUtc = DateTime.UtcNow;
                eventSchedules.Add(scheduleDetail);
                return eventSchedules;
            }
            foreach (var scheduleDetail in eventSchedules)
            {
                if (scheduleDetail.StartDateTime.Date >= startDateTime.Date && scheduleDetail.EndDateTime.Date <= endDateTime.Date)
                {
                    scheduleDetail.StartDateTime = _commonUtilityProvider.GetUtcDate(scheduleDetail.StartDateTime, eventRecurranceCommand.LocalStartTime, eventRecurranceCommand.TimeZoneOffSet);
                    scheduleDetail.EndDateTime = _commonUtilityProvider.GetUtcDate(scheduleDetail.EndDateTime, eventRecurranceCommand.LocalEndTime, eventRecurranceCommand.TimeZoneOffSet);
                    scheduleDetail.UpdatedBy = eventRecurranceCommand.ModifiedBy;
                    scheduleDetail.ModifiedBy = eventRecurranceCommand.ModifiedBy;
                    scheduleDetail.UpdatedUtc = DateTime.UtcNow;
                    eventSchedules1.Add(scheduleDetail);
                }
            }
            return eventSchedules;
        }
    }
}