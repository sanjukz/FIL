using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CreateEventV1
{
    public class SavePerformanceDetailCommandHandler : BaseCommandHandlerWithResult<EventPerformanceCommand, EventPerformanceCommandResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ILiveEventDetailRepository _liveEventDetailRepository;
        private readonly IStepProvider _stepProvider;
        private readonly ILogger _logger;

        public SavePerformanceDetailCommandHandler(
             IEventRepository eventRepository,
             ILiveEventDetailRepository liveEventDetailRepository,
             IEventDetailRepository eventDetailRepository,
            IStepProvider stepProvider,
            ILogger logger,
            IMediator mediator)
            : base(mediator)
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _liveEventDetailRepository = liveEventDetailRepository;
            _stepProvider = stepProvider;
            _logger = logger;
        }

        public FIL.Contracts.DataModels.LiveEventDetail SaveLiveEventDetail(
            EventPerformanceCommand command,
            FIL.Contracts.DataModels.EventDetail eventDetail
            )
        {
            var liveEventDetails = _liveEventDetailRepository.Get(command.PerformanceTypeModel.Id);
            FIL.Contracts.DataModels.LiveEventDetail LiveEventDetailModel = new FIL.Contracts.DataModels.LiveEventDetail();
            LiveEventDetailModel.Id = liveEventDetails != null ? liveEventDetails.Id : 0;
            LiveEventDetailModel.OnlineEventTypeId = (OnlineEventTypes)command.PerformanceTypeModel.OnlineEventTypeId;
            LiveEventDetailModel.PerformanceTypeId = command.PerformanceTypeModel.PerformanceTypeId;
            LiveEventDetailModel.EventId = command.EventId;
            LiveEventDetailModel.IsVideoUploaded = command.PerformanceTypeModel.IsVideoUploaded;
            LiveEventDetailModel.EventStartDateTime = eventDetail.StartDateTime;
            LiveEventDetailModel.IsEnabled = command.PerformanceTypeModel.IsEnabled;
            LiveEventDetailModel.VideoId = "";
            LiveEventDetailModel.CreatedUtc = liveEventDetails != null ? liveEventDetails.CreatedUtc : DateTime.UtcNow;
            LiveEventDetailModel.UpdatedUtc = DateTime.UtcNow;
            LiveEventDetailModel.CreatedBy = liveEventDetails != null ? liveEventDetails.CreatedBy : command.ModifiedBy;
            LiveEventDetailModel.UpdatedBy = command.ModifiedBy;
            LiveEventDetailModel.ModifiedBy = command.ModifiedBy;
            return _liveEventDetailRepository.Save(LiveEventDetailModel);
        }

        protected override async Task<ICommandResult> Handle(EventPerformanceCommand command)
        {
            try
            {
                var eventDetail = _eventDetailRepository.GetByEventId(command.EventId);
                var liveEventDetail = SaveLiveEventDetail(command, eventDetail);
                var eventData = _eventRepository.Get(eventDetail.EventId);
                command.PerformanceTypeModel.Id = liveEventDetail.Id;
                command.PerformanceTypeModel.EventId = command.EventId;
                var eventStepDetail = _stepProvider.SaveEventStepDetails(command.EventId, command.CurrentStep, true, command.ModifiedBy);
                return new EventPerformanceCommandResult
                {
                    Success = true,
                    EventId = command.EventId,
                    EventAltId = eventData.AltId,
                    OnlineEventType = command.PerformanceTypeModel.OnlineEventTypeId.ToString(),
                    CompletedStep = eventStepDetail.CompletedStep,
                    CurrentStep = eventStepDetail.CurrentStep,
                    PerformanceTypeModel = command.PerformanceTypeModel
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
                return new EventPerformanceCommandResult { };
            }
        }
    }
}