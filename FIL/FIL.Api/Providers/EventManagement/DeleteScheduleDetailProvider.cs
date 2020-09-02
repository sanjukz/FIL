using FIL.Api.Repositories;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FIL.Api.Providers.EventManagement
{
    public interface IDeleteScheduleDetailProvider
    {
        List<FIL.Contracts.DataModels.ScheduleDetail> GetScheduleDetails(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand);

        void DeleteEventSchedule(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand);
    }

    public class DeleteScheduleDetailProvider : IDeleteScheduleDetailProvider
    {
        private IEventScheduleRepository _eventScheduleRepository;
        private IScheduleDetailRepository _scheduleDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ILocalTimeZoneConvertProvider _localTimeZoneConvertProvider;

        public DeleteScheduleDetailProvider(
            IEventScheduleRepository eventScheduleRepository,
            IScheduleDetailRepository scheduleDetailRepository,
            IEventAttributeRepository eventAttributeRepository,
            ILocalTimeZoneConvertProvider localTimeZoneConvertProvider,
            IEventDetailRepository eventDetailRepository
            )
        {
            _eventScheduleRepository = eventScheduleRepository;
            _scheduleDetailRepository = scheduleDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _eventDetailRepository = eventDetailRepository;
            _localTimeZoneConvertProvider = localTimeZoneConvertProvider;
        }

        public void DeleteEventSchedule(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand)
        {
            var eventSchedule = _eventScheduleRepository.Get(eventRecurranceCommand.EventScheduleId);
            _eventScheduleRepository.Delete(eventSchedule);
        }

        public List<FIL.Contracts.DataModels.ScheduleDetail> GetScheduleDetails(FIL.Contracts.Commands.CreateEventV1.EventRecurranceCommand eventRecurranceCommand)
        {
            List<FIL.Contracts.DataModels.ScheduleDetail> eventSchedules = new List<Contracts.DataModels.ScheduleDetail>();

            if (eventRecurranceCommand.ActionType == Contracts.Commands.CreateEventV1.ActionType.BulkDelete)
            {
                var alleventSchedules = _eventScheduleRepository.GetAllByEventId(eventRecurranceCommand.EventId);
                var eventDetails = _eventDetailRepository.GetByEventId(eventRecurranceCommand.EventId);
                var eventAttributes = _eventAttributeRepository.GetByEventDetailId(eventDetails.Id);
                var StartDate = eventRecurranceCommand.StartDateTime; // new DateTime(eventRecurranceCommand.StartDateTime.Year, eventRecurranceCommand.StartDateTime.Month, eventRecurranceCommand.StartDateTime.Day).ToUniversalTime();
                var EndDate = eventRecurranceCommand.EndDateTime; // new DateTime(eventRecurranceCommand.EndDateTime.Year, eventRecurranceCommand.EndDateTime.Month, eventRecurranceCommand.EndDateTime.Day).ToUniversalTime();
                var scheduleDetails = _scheduleDetailRepository.GetAllByEventScheduleIds(alleventSchedules.Select(s => s.Id).ToList()).Where(s => s.StartDateTime.Date >= StartDate.Date && s.EndDateTime.Date <= EndDate.Date).OrderBy(s => s.StartDateTime).ToList();

                foreach (var scheduleDetail in scheduleDetails)
                {
                    var startDate = _localTimeZoneConvertProvider.ConvertToLocal(scheduleDetail.StartDateTime, eventAttributes.TimeZone);
                    var endDate = _localTimeZoneConvertProvider.ConvertToLocal(scheduleDetail.EndDateTime, eventAttributes.TimeZone);
                    if (startDate.ToString(@"HH:mm", new CultureInfo("en-US")) == eventRecurranceCommand.LocalStartTime && endDate.ToString(@"HH:mm", new CultureInfo("en-US")) == eventRecurranceCommand.LocalEndTime)
                    {
                        eventSchedules.Add(scheduleDetail);
                    }
                }
                return eventSchedules;
                //  return scheduleDetails.Where(s => s.StartDateTime.ToString(@"HH:mm", new CultureInfo("en-US")) == eventRecurranceCommand.StartDateTime.ToString(@"HH:mm", new CultureInfo("en-US")) && s.EndDateTime.ToString(@"HH:mm", new CultureInfo("en-US")) == eventRecurranceCommand.EndDateTime.ToString(@"HH:mm", new CultureInfo("en-US"))).ToList();
            }
            else
            {
                var scheduleDetail = _scheduleDetailRepository.Get(eventRecurranceCommand.ScheduleDetailId);
                eventSchedules.Add(scheduleDetail);
                return eventSchedules;
            }
        }
    }
}