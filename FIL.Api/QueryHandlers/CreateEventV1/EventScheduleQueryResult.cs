using FIL.Api.Providers;
using FIL.Api.Providers.EventManagement;
using FIL.Api.Repositories;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;
using System.Globalization;
using System.Linq;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class EventScheduleQueryHandler : IQueryHandler<EventScheduleQuery, EventScheduleQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IDayTimeMappingsRepository _dayTimeMappingsRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IGetScheduleDetailProvider _getScheduleDetailProvider;
        private readonly ILocalTimeZoneConvertProvider _localTimeZoneConvertProvider;

        public EventScheduleQueryHandler(IEventRepository eventRepository,
             IEventDetailRepository eventDetailRepository,
             IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepositor,
             IEventAttributeRepository eventAttributeRepository,
             IGetScheduleDetailProvider getScheduleDetailProvider,
            ILocalTimeZoneConvertProvider localTimeZoneConvertProvider,
              IDayTimeMappingsRepository dayTimeMappingsRepository)
        {
            _eventRepository = eventRepository;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepositor;
            _dayTimeMappingsRepository = dayTimeMappingsRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _getScheduleDetailProvider = getScheduleDetailProvider;
            _localTimeZoneConvertProvider = localTimeZoneConvertProvider;
        }

        public EventScheduleQueryResult Handle(EventScheduleQuery query)
        {
            try
            {
                FIL.Contracts.Models.CreateEventV1.EventScheduleModel eventScheduleModel = new FIL.Contracts.Models.CreateEventV1.EventScheduleModel();
                var currentEvent = _eventRepository.Get(query.EventId);
                if (currentEvent == null)
                {
                    return new EventScheduleQueryResult { Success = true, EventScheduleModel = eventScheduleModel };
                }
                var currentEventDetail = _eventDetailRepository.GetByEventId(query.EventId);
                if (currentEventDetail == null)
                {
                    eventScheduleModel.EventFrequencyType = Contracts.Enums.EventFrequencyType.Single;
                    eventScheduleModel.StartDateTime = DateTime.UtcNow;
                    eventScheduleModel.EndDateTime = DateTime.UtcNow;
                    return new EventScheduleQueryResult
                    {
                        Success = true,
                        IsValidLink = true,
                        IsDraft = true,
                        EventScheduleModel = eventScheduleModel
                    };
                }
                var eventAttribute = _eventAttributeRepository.GetByEventDetailId(currentEventDetail.Id);
                var placeWeekOpenDay = _placeWeekOpenDaysRepository.GetByEventId(currentEvent.Id).FirstOrDefault();
                var dayTImeMappings = _dayTimeMappingsRepository.GetAllByPlaceWeekOpenDay(placeWeekOpenDay.Id).FirstOrDefault();
                eventScheduleModel.DayId = (int)placeWeekOpenDay.DayId;
                eventScheduleModel.StartDateTime = currentEventDetail.StartDateTime;
                eventScheduleModel.EndDateTime = currentEventDetail.EndDateTime;
                eventScheduleModel.EventDetailId = currentEventDetail.Id;
                eventScheduleModel.IsEnabled = currentEventDetail.IsEnabled;
                eventScheduleModel.LocalStartTime = dayTImeMappings.StartTime;
                eventScheduleModel.LocalEndTime = dayTImeMappings.EndTime;
                eventScheduleModel.TimeZoneAbbrivation = eventAttribute.TimeZoneAbbreviation;
                eventScheduleModel.TimeZoneOffset = eventAttribute.TimeZone;
                eventScheduleModel.EventId = currentEvent.Id;
                eventScheduleModel.EventFrequencyType = currentEventDetail.EventFrequencyType;
                if (currentEventDetail.EventFrequencyType == Contracts.Enums.EventFrequencyType.Recurring)
                {
                    eventScheduleModel.StartDateTime = currentEventDetail.StartDateTime;
                    eventScheduleModel.EndDateTime = currentEventDetail.EndDateTime;
                    eventScheduleModel.LocalStartTime = _localTimeZoneConvertProvider.ConvertToLocal(currentEventDetail.StartDateTime, eventAttribute.TimeZone).ToString(@"HH:mm", new CultureInfo("en-US"));
                    eventScheduleModel.LocalEndTime = _localTimeZoneConvertProvider.ConvertToLocal(currentEventDetail.EndDateTime, eventAttribute.TimeZone).ToString(@"HH:mm", new CultureInfo("en-US"));
                }
                else
                {
                    eventScheduleModel.EventFrequencyType = currentEventDetail.EventFrequencyType;
                }
                return new EventScheduleQueryResult
                {
                    Success = true,
                    IsDraft = false,
                    IsValidLink = true,
                    EventScheduleModel = eventScheduleModel
                };
            }
            catch (Exception e)
            {
                return new EventScheduleQueryResult
                {
                    IsValidLink = true,
                    IsDraft = false
                };
            }
        }
    }
}