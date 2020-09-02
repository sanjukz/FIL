using FIL.Api.Providers;
using FIL.Api.Providers.EventManagement;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CreateEventV1
{
    public class SaveEventScheduleCommandHandler : BaseCommandHandlerWithResult<EventScheduleCommand, EventScheduleCommandResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IDayTimeMappingsRepository _dayTimeMappingsRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IStepProvider _stepProvider;
        private readonly IGetScheduleDetailProvider _getScheduleDetailProvider;
        private readonly ILiveEventDetailRepository _liveEventDetailRepository;
        private readonly ILogger _logger;

        public SaveEventScheduleCommandHandler(
              IEventDetailRepository eventDetailRepository,
            IEventRepository eventRepository,
             IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository,
            IDayTimeMappingsRepository dayTimeMappingsRepository,
            IEventAttributeRepository eventAttributeRepository,
            IGetScheduleDetailProvider getScheduleDetailProvider,
            ILiveEventDetailRepository liveEventDetailRepository,
            IStepProvider stepProvider,
            ILogger logger,
            IMediator mediator)
            : base(mediator)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepository;
            _dayTimeMappingsRepository = dayTimeMappingsRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _getScheduleDetailProvider = getScheduleDetailProvider;
            _liveEventDetailRepository = liveEventDetailRepository;
            _stepProvider = stepProvider;
        }

        protected void UpdateLiveEventDetails(EventDetail eventDetail)
        {
            var liveEventDetail = _liveEventDetailRepository.GetByEventId(eventDetail.Id);
            if (liveEventDetail != null)
            {
                liveEventDetail.EventStartDateTime = eventDetail.StartDateTime;
                _liveEventDetailRepository.Save(liveEventDetail);
            }
        }

        protected void SaveEventAttribute(EventScheduleCommand command,
           EventDetail eventDetail)
        {
            EventAttribute eventAttribute = new EventAttribute();
            var eventTicketAttribute = _eventAttributeRepository.GetByEventDetailId(eventDetail.Id);
            if (eventTicketAttribute != null)
            {
                _eventAttributeRepository.Delete(eventTicketAttribute);
            }
            var eventAttributes = new EventAttribute
            {
                TimeZoneAbbreviation = command.EventScheduleModel.TimeZoneAbbrivation,
                EventDetailId = eventDetail.Id,
                CreatedBy = command.ModifiedBy,
                CreatedUtc = DateTime.UtcNow,
                IsEnabled = true,
                TimeZone = command.EventScheduleModel.TimeZoneOffset,
                ModifiedBy = command.ModifiedBy,
                UpdatedBy = command.ModifiedBy,
                UpdatedUtc = DateTime.UtcNow
            };
            _eventAttributeRepository.Save(eventAttributes);
        }

        protected FIL.Contracts.DataModels.DayTimeMappings SaveDayTimeMappings(EventScheduleCommand command,
            PlaceWeekOpenDays placeWeekOpenDays)
        {
            DayTimeMappings dayTimeMapping = new DayTimeMappings();
            dayTimeMapping.AltId = Guid.NewGuid();
            dayTimeMapping.StartTime = command.EventScheduleModel.LocalStartTime;
            dayTimeMapping.EndTime = command.EventScheduleModel.LocalEndTime;
            dayTimeMapping.PlaceWeekOpenDayId = placeWeekOpenDays.Id;
            dayTimeMapping.IsEnabled = true;
            dayTimeMapping.CreatedUtc = DateTime.UtcNow;
            dayTimeMapping.UpdatedUtc = DateTime.UtcNow;
            dayTimeMapping.CreatedBy = command.ModifiedBy;
            dayTimeMapping.UpdatedBy = command.ModifiedBy;
            dayTimeMapping.ModifiedBy = command.ModifiedBy;
            return _dayTimeMappingsRepository.Save(dayTimeMapping);
        }

        protected FIL.Contracts.DataModels.PlaceWeekOpenDays SavePlaceWeekOpenDays(EventScheduleCommand command, Event currentEvent)
        {
            PlaceWeekOpenDays currentPlaceWeekOpenDay = new PlaceWeekOpenDays();
            currentPlaceWeekOpenDay.AltId = Guid.NewGuid();
            currentPlaceWeekOpenDay.DayId = command.EventScheduleModel.StartDateTime.DayOfWeek == System.DayOfWeek.Sunday ? 7 : (int)command.EventScheduleModel.StartDateTime.DayOfWeek;
            currentPlaceWeekOpenDay.EventId = currentEvent.Id;
            currentPlaceWeekOpenDay.IsSameTime = true;
            currentPlaceWeekOpenDay.IsEnabled = true;
            currentPlaceWeekOpenDay.CreatedUtc = DateTime.UtcNow;
            currentPlaceWeekOpenDay.UpdatedUtc = DateTime.UtcNow;
            currentPlaceWeekOpenDay.CreatedBy = command.ModifiedBy;
            currentPlaceWeekOpenDay.ModifiedBy = command.ModifiedBy;
            return _placeWeekOpenDaysRepository.Save(currentPlaceWeekOpenDay);
        }

        protected FIL.Contracts.DataModels.EventDetail SaveEventDetail(EventScheduleCommand command, Event currentEvent)
        {
            if (command.EventScheduleModel.EventFrequencyType == Contracts.Enums.EventFrequencyType.Recurring)
            {
                var eventScheduleDetail = _getScheduleDetailProvider.GetScheduleDetails(currentEvent.Id, new DateTime(), new DateTime(), true).FirstOrDefault();
                if (eventScheduleDetail != null)
                {
                    command.EventScheduleModel.StartDateTime = eventScheduleDetail.StartDateTime;
                    command.EventScheduleModel.EndDateTime = eventScheduleDetail.EndDateTime;
                }
                else
                {
                    command.EventScheduleModel.StartDateTime = DateTime.UtcNow;
                    command.EventScheduleModel.EndDateTime = DateTime.UtcNow;
                }
            }
            EventDetail eventDetail = new EventDetail();
            eventDetail = command.EventScheduleModel.EventDetailId != 0 ? _eventDetailRepository.Get(command.EventScheduleModel.EventDetailId) : eventDetail;
            eventDetail.AltId = command.EventScheduleModel.EventDetailId != 0 ? eventDetail.AltId : Guid.NewGuid();
            eventDetail.Id = command.EventScheduleModel.EventDetailId != 0 ? eventDetail.Id : 0;
            eventDetail.EventId = command.EventScheduleModel.EventId;
            eventDetail.Description = "NA";
            eventDetail.IsEnabled = true;
            eventDetail.Name = currentEvent.Name;
            eventDetail.VenueId = command.EventScheduleModel.VenueId;
            eventDetail.IsEnabled = command.EventScheduleModel.IsEnabled;
            eventDetail.StartDateTime = command.EventScheduleModel.StartDateTime;
            eventDetail.EndDateTime = command.EventScheduleModel.EndDateTime;
            eventDetail.GroupId = 1;
            eventDetail.EventFrequencyType = command.EventScheduleModel.EventFrequencyType;
            eventDetail.CreatedUtc = eventDetail.Id != 0 ? eventDetail.CreatedUtc : DateTime.UtcNow;
            eventDetail.UpdatedUtc = eventDetail.Id != 0 ? DateTime.UtcNow : eventDetail.CreatedUtc;
            eventDetail.CreatedBy = eventDetail.Id != 0 ? eventDetail.CreatedBy : command.ModifiedBy;
            eventDetail.UpdatedBy = command.ModifiedBy;
            eventDetail.ModifiedBy = command.ModifiedBy;
            return _eventDetailRepository.Save(eventDetail);
        }

        protected override async Task<ICommandResult> Handle(EventScheduleCommand command)
        {
            try
            {
                var currentEvent = _eventRepository.Get(command.EventScheduleModel.EventId);
                var currentEventDetail = _eventDetailRepository.GetByEventId(command.EventScheduleModel.EventId);
                var placeWeekOpen = _placeWeekOpenDaysRepository.GetByEventId(command.EventScheduleModel.EventId);
                var dayTimeMappings = _dayTimeMappingsRepository.GetAllByPlaceWeekOpenDays(placeWeekOpen.Select(s => s.Id).ToList());
                if (currentEventDetail != null && command.EventFrequencyType == Contracts.Enums.EventFrequencyType.Recurring)
                {
                    return new EventScheduleCommandResult
                    {
                        Success = true,
                        EventScheduleModel = command.EventScheduleModel
                    };
                }
                if (currentEventDetail != null && command.EventScheduleModel.EventDetailId == 0)
                {
                    command.EventScheduleModel.EventDetailId = currentEventDetail.Id;
                }
                foreach (var currentDayTimeMapping in dayTimeMappings)
                {
                    _dayTimeMappingsRepository.Delete(currentDayTimeMapping);
                }
                foreach (var currentPlaceWeekOpen in placeWeekOpen)
                {
                    _placeWeekOpenDaysRepository.Delete(currentPlaceWeekOpen);
                }
                var eventDetail = SaveEventDetail(command, currentEvent);
                var placeWeekOpenDays = SavePlaceWeekOpenDays(command, currentEvent);
                SaveDayTimeMappings(command, placeWeekOpenDays);
                SaveEventAttribute(command, eventDetail);
                UpdateLiveEventDetails(eventDetail);
                command.EventScheduleModel.EventDetailId = eventDetail.Id;
                var eventStepDetail = _stepProvider.SaveEventStepDetails(eventDetail.EventId, command.CurrentStep, true, command.ModifiedBy);
                return new EventScheduleCommandResult
                {
                    Success = true,
                    CompletedStep = eventStepDetail.CompletedStep,
                    CurrentStep = eventStepDetail.CurrentStep,
                    EventScheduleModel = command.EventScheduleModel
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
                return new EventScheduleCommandResult { };
            }
        }
    }
}