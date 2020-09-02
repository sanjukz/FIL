using FIL.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FIL.Api.Providers.EventManagement
{
    public interface IGetScheduleDetailProvider
    {
        List<FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel> GetScheduleDetails(long EventId,
            DateTime StartDate,
            DateTime EndDate,
            bool IsGetDateRange = false,
            bool IsGetAll = false);
    }

    public class GetScheduleDetailProvider : IGetScheduleDetailProvider
    {
        private readonly IEventScheduleRepository _eventScheduleRepository;
        private readonly IScheduleDetailRepository _scheduleDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ILocalTimeZoneConvertProvider _localTimeZoneConvertProvider;

        public GetScheduleDetailProvider(
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

        public List<FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel> GetScheduleDetails(long EventId,
            DateTime StartDate,
            DateTime EndDate,
            bool IsGetDateRange = false,
            bool IsGetAll = false
            )
        {
            List<FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel> eventRecurranceScheduleModels = new List<FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel>();
            var allScheduleDetails = new List<FIL.Contracts.DataModels.ScheduleDetail>();
            var eventSchedules = _eventScheduleRepository.GetAllByEventId(EventId);
            var eventDetails = _eventDetailRepository.GetByEventId(EventId);
            if (eventDetails == null && IsGetDateRange)
            {
                return eventRecurranceScheduleModels;
            }
            var eventAttributes = _eventAttributeRepository.GetByEventDetailId(eventDetails.Id);
            if (IsGetDateRange)
            {
                allScheduleDetails = _scheduleDetailRepository.GetAllByEventScheduleIds(eventSchedules.Select(s => s.Id).ToList()).ToList();
                FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel eventRecurranceScheduleModel = new FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel();
                if (!allScheduleDetails.Any())
                {
                    return eventRecurranceScheduleModels;
                }
                eventRecurranceScheduleModel.StartDateTime = allScheduleDetails.OrderBy(s => s.StartDateTime).FirstOrDefault().StartDateTime;
                eventRecurranceScheduleModel.EndDateTime = allScheduleDetails.OrderBy(s => s.StartDateTime).LastOrDefault().EndDateTime;
                eventRecurranceScheduleModels.Add(eventRecurranceScheduleModel);
                return eventRecurranceScheduleModels;
            }
            allScheduleDetails = _scheduleDetailRepository.GetAllByEventScheduleIds(eventSchedules.Select(s => s.Id).ToList()).OrderBy(s => s.StartDateTime).ToList();
            var scheduleDetails = allScheduleDetails.OrderBy(s => s.StartDateTime).ToList();
            if (!IsGetAll)
            {
                scheduleDetails = allScheduleDetails.Where(s => s.StartDateTime >= new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, 00, 00, 00).ToUniversalTime() && s.EndDateTime <= new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 00).ToUniversalTime()).OrderBy(s => s.StartDateTime).ToList();
            }
            foreach (var scheduleDetail in scheduleDetails)
            {
                FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel eventRecurranceScheduleModel = new FIL.Contracts.Models.CreateEventV1.EventRecurranceScheduleModel();
                var currentEventSchedule = eventSchedules.Where(s => s.Id == scheduleDetail.EventScheduleId).FirstOrDefault();
                var startDate = _localTimeZoneConvertProvider.ConvertToLocal(scheduleDetail.StartDateTime, eventAttributes.TimeZone);
                var endDate = _localTimeZoneConvertProvider.ConvertToLocal(scheduleDetail.EndDateTime, eventAttributes.TimeZone);
                eventRecurranceScheduleModel.EventScheduleId = currentEventSchedule.Id;
                eventRecurranceScheduleModel.DayIds = currentEventSchedule.DayId;
                eventRecurranceScheduleModel.ScheduleDetailId = scheduleDetail.Id;
                eventRecurranceScheduleModel.IsEnabled = scheduleDetail.IsEnabled;
                eventRecurranceScheduleModel.StartDateTime = scheduleDetail.StartDateTime;
                eventRecurranceScheduleModel.EndDateTime = scheduleDetail.EndDateTime;
                eventRecurranceScheduleModel.LocalStartDateTime = startDate;
                eventRecurranceScheduleModel.LocalEndDateTime = endDate;
                eventRecurranceScheduleModel.EventScheduleStartDateTime = allScheduleDetails.FirstOrDefault().StartDateTime;
                eventRecurranceScheduleModel.EventScheduleEndDateTime = allScheduleDetails.LastOrDefault().EndDateTime;
                eventRecurranceScheduleModel.LocalStartTime = startDate.ToString(@"HH:mm", new CultureInfo("en-US"));
                eventRecurranceScheduleModel.LocalEndTime = endDate.ToString(@"HH:mm", new CultureInfo("en-US"));
                eventRecurranceScheduleModel.LocalStartDateString = startDate.DayOfWeek + ", " + startDate.ToString(@"MMM dd, yyyy", new CultureInfo("en-US"));
                eventRecurranceScheduleModel.LocalEndDateString = endDate.DayOfWeek + ", " + endDate.ToString(@"MMM dd, yyyy", new CultureInfo("en-US"));
                eventRecurranceScheduleModel.LocalEventScheduleStartDateTimeString = allScheduleDetails.FirstOrDefault().StartDateTime.DayOfWeek + ", " + allScheduleDetails.FirstOrDefault().StartDateTime.ToString(@"MMM dd, yyyy", new CultureInfo("en-US"));
                eventRecurranceScheduleModel.LocalEventScheduleEndDateTimeString = allScheduleDetails.LastOrDefault().EndDateTime.DayOfWeek + ", " + allScheduleDetails.LastOrDefault().EndDateTime.ToString(@"MMM dd, yyyy", new CultureInfo("en-US"));
                eventRecurranceScheduleModels.Add(eventRecurranceScheduleModel);
            }
            return eventRecurranceScheduleModels;
        }
    }
}