using FIL.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers.EventManagement
{
    public interface ISaveScheduleDetailProvider
    {
        List<FIL.Contracts.DataModels.ScheduleDetail> GetScheduleDetails(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand);
    }

    public class SaveScheduleDetailProvider : ISaveScheduleDetailProvider
    {
        private readonly ICommonUtilityProvider _commonUtilityProvider;
        private readonly IEventScheduleRepository _eventScheduleRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IScheduleDetailRepository _scheduleDetailRepository;

        public SaveScheduleDetailProvider(
            ICommonUtilityProvider commonUtilityProvider,
            IEventScheduleRepository eventScheduleRepository,
            IScheduleDetailRepository scheduleDetailRepository,
            IEventDetailRepository eventDetailRepository
            )
        {
            _commonUtilityProvider = commonUtilityProvider;
            _eventScheduleRepository = eventScheduleRepository;
            _eventDetailRepository = eventDetailRepository;
            _scheduleDetailRepository = scheduleDetailRepository;
        }

        private FIL.Contracts.DataModels.EventSchedule SaveEventSchedule(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand)
        {
            var eventSchedule = new FIL.Contracts.DataModels.EventSchedule
            {
                EventId = eventRecurranceCommand.EventId,
                DayId = eventRecurranceCommand.DayIds,
                EventFrequencyTypeId = eventRecurranceCommand.EventFrequencyType,
                OccuranceTypeId = eventRecurranceCommand.OccuranceType,
                StartDateTime = _commonUtilityProvider.GetUtcDate(eventRecurranceCommand.StartDateTime, eventRecurranceCommand.LocalStartTime, eventRecurranceCommand.TimeZoneOffSet),
                EndDateTime = _commonUtilityProvider.GetUtcDate(eventRecurranceCommand.EndDateTime, eventRecurranceCommand.LocalEndTime, eventRecurranceCommand.TimeZoneOffSet),
                Name = eventRecurranceCommand.StartDateTime.ToString() + " - " + eventRecurranceCommand.EndDateTime.ToString(),
                IsEnabled = true,
                CreatedBy = eventRecurranceCommand.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                ModifiedBy = eventRecurranceCommand.ModifiedBy,
                UpdatedBy = eventRecurranceCommand.ModifiedBy,
                UpdatedUtc = DateTime.UtcNow
            };
            return _eventScheduleRepository.Save(eventSchedule);
        }

        public List<FIL.Contracts.DataModels.ScheduleDetail> GetScheduleDetails(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand)
        {
            var eventSchedule = SaveEventSchedule(eventRecurranceCommand);
            List<FIL.Contracts.DataModels.ScheduleDetail> eventSchedules = new List<Contracts.DataModels.ScheduleDetail>();
            var eventSchedules1 = _eventScheduleRepository.GetAllByEventId(eventRecurranceCommand.EventId);
            var scheduleDetails = _scheduleDetailRepository.GetAllByEventScheduleIds(eventSchedules1.Select(s => s.Id).ToList());
            if (eventRecurranceCommand.OccuranceType == Contracts.Enums.OccuranceType.Once)
            {
                var startDateTime = _commonUtilityProvider.GetUtcDate(eventRecurranceCommand.StartDateTime, eventRecurranceCommand.LocalStartTime, eventRecurranceCommand.TimeZoneOffSet);
                var endDateTime = _commonUtilityProvider.GetUtcDate(eventRecurranceCommand.EndDateTime, eventRecurranceCommand.LocalEndTime, eventRecurranceCommand.TimeZoneOffSet);
                if (!scheduleDetails.Where(s => s.StartDateTime == startDateTime && s.EndDateTime == endDateTime).Any())
                {
                    // allow only one day insert
                    FIL.Contracts.DataModels.ScheduleDetail scheduleDetail = new Contracts.DataModels.ScheduleDetail
                    {
                        EventScheduleId = eventSchedule.Id,
                        StartDateTime = startDateTime,
                        EndDateTime = endDateTime,
                        CreatedBy = eventRecurranceCommand.ModifiedBy,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        ModifiedBy = eventRecurranceCommand.ModifiedBy,
                        UpdatedBy = eventRecurranceCommand.ModifiedBy,
                        UpdatedUtc = DateTime.UtcNow
                    };
                    eventSchedules.Add(scheduleDetail);
                    return eventSchedules;
                }
            }
            else if (eventRecurranceCommand.OccuranceType == Contracts.Enums.OccuranceType.Daily)
            {
                foreach (DateTime day in _commonUtilityProvider.EachDay(eventRecurranceCommand.StartDateTime, eventRecurranceCommand.EndDateTime))
                {
                    var startDateTime = _commonUtilityProvider.GetUtcDate(day, eventRecurranceCommand.LocalStartTime, eventRecurranceCommand.TimeZoneOffSet);
                    var endDateTime = _commonUtilityProvider.GetUtcDate(day, eventRecurranceCommand.LocalEndTime, eventRecurranceCommand.TimeZoneOffSet);
                    if (scheduleDetails.Where(s => s.StartDateTime == startDateTime && s.EndDateTime == endDateTime).Any())
                    {
                        continue;
                    }
                    // allow all day insert
                    FIL.Contracts.DataModels.ScheduleDetail scheduleDetail = new Contracts.DataModels.ScheduleDetail
                    {
                        EventScheduleId = eventSchedule.Id,
                        StartDateTime = startDateTime,
                        EndDateTime = endDateTime,
                        CreatedBy = eventRecurranceCommand.ModifiedBy,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        ModifiedBy = eventRecurranceCommand.ModifiedBy,
                        UpdatedBy = eventRecurranceCommand.ModifiedBy,
                        UpdatedUtc = DateTime.UtcNow
                    };
                    eventSchedules.Add(scheduleDetail);
                }
                return eventSchedules;
            }
            else if (eventRecurranceCommand.OccuranceType == Contracts.Enums.OccuranceType.Weekly)
            {
                foreach (DateTime day in _commonUtilityProvider.EachDay(eventRecurranceCommand.StartDateTime, eventRecurranceCommand.EndDateTime))
                {
                    var startDateTime = _commonUtilityProvider.GetUtcDate(day, eventRecurranceCommand.LocalStartTime, eventRecurranceCommand.TimeZoneOffSet);
                    var endDateTime = _commonUtilityProvider.GetUtcDate(day, eventRecurranceCommand.LocalEndTime, eventRecurranceCommand.TimeZoneOffSet);
                    if (scheduleDetails.Where(s => s.StartDateTime == startDateTime && s.EndDateTime == endDateTime).Any())
                    {
                        continue;
                    }
                    // If Day match with the dayIds
                    if (eventRecurranceCommand.DayIds.Contains(((int)day.DayOfWeek).ToString()))
                    {
                        FIL.Contracts.DataModels.ScheduleDetail scheduleDetail = new Contracts.DataModels.ScheduleDetail
                        {
                            EventScheduleId = eventSchedule.Id,
                            StartDateTime = startDateTime,
                            EndDateTime = endDateTime,
                            CreatedBy = eventRecurranceCommand.ModifiedBy,
                            IsEnabled = true,
                            CreatedUtc = DateTime.UtcNow,
                            ModifiedBy = eventRecurranceCommand.ModifiedBy,
                            UpdatedBy = eventRecurranceCommand.ModifiedBy,
                            UpdatedUtc = DateTime.UtcNow
                        };
                        eventSchedules.Add(scheduleDetail);
                    }
                }
                return eventSchedules;
            }
            else if (eventRecurranceCommand.OccuranceType == Contracts.Enums.OccuranceType.Monthly)
            {
                foreach (DateTime day in _commonUtilityProvider.EachDay(eventRecurranceCommand.StartDateTime, eventRecurranceCommand.EndDateTime))
                {
                    var startDateTime = _commonUtilityProvider.GetUtcDate(day, eventRecurranceCommand.LocalStartTime, eventRecurranceCommand.TimeZoneOffSet);
                    var endDateTime = _commonUtilityProvider.GetUtcDate(day, eventRecurranceCommand.LocalEndTime, eventRecurranceCommand.TimeZoneOffSet);
                    if (scheduleDetails.Where(s => s.StartDateTime == startDateTime && s.EndDateTime == endDateTime).Any())
                    {
                        continue;
                    }
                    // If Day match with the loop day
                    if (eventRecurranceCommand.StartDateTime.Day == day.Day)
                    {
                        FIL.Contracts.DataModels.ScheduleDetail scheduleDetail = new Contracts.DataModels.ScheduleDetail
                        {
                            EventScheduleId = eventSchedule.Id,
                            StartDateTime = startDateTime,
                            EndDateTime = endDateTime,
                            CreatedBy = eventRecurranceCommand.ModifiedBy,
                            IsEnabled = true,
                            CreatedUtc = DateTime.UtcNow,
                            ModifiedBy = eventRecurranceCommand.ModifiedBy,
                            UpdatedBy = eventRecurranceCommand.ModifiedBy,
                            UpdatedUtc = DateTime.UtcNow
                        };
                        eventSchedules.Add(scheduleDetail);
                    }
                }
                return eventSchedules;
            }
            return eventSchedules;
        }
    }
}