using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class StepQueryHandler : IQueryHandler<StepQuery, StepsQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStepDetailsRepository _stepDetailsRepository;
        private readonly IEventStepDetailRepository _eventStepDetailRepository;
        private readonly IStepRepository _stepRepository;
        private readonly IStepProvider _stepProvider;
        private readonly IZoomUserRepository _zoomUserRepository;
        private readonly IEventDetailRepository _eventDetailRepository;

        public StepQueryHandler(IEventRepository eventRepository,
            IStepDetailsRepository stepDetailsRepository,
            IEventStepDetailRepository eventStepDetailRepository,
            IStepProvider stepProvider,
            IZoomUserRepository zoomUserRepository,
            IEventDetailRepository eventDetailRepository,
            IStepRepository stepRepository
            )
        {
            _eventRepository = eventRepository;
            _stepRepository = stepRepository;
            _stepDetailsRepository = stepDetailsRepository;
            _eventStepDetailRepository = eventStepDetailRepository;
            _stepProvider = stepProvider;
            _eventDetailRepository = eventDetailRepository;
            _zoomUserRepository = zoomUserRepository;
        }

        public StepsQueryResult Handle(StepQuery query)
        {
            try
            {
                List<FIL.Contracts.Models.CreateEventV1.StepModel> stepModels = new List<FIL.Contracts.Models.CreateEventV1.StepModel>();
                short currentStep = 1;
                string completedSteps = "", eventName = "";
                var eventStatus = FIL.Contracts.Enums.EventStatus.None;
                var stepDetails = _stepDetailsRepository.GetAll().Where(s => s.MasterEventTypeId == query.MasterEventType && s.IsEnabled);
                var steps = _stepRepository.GetByIds(stepDetails.Select(s => s.StepId).ToList());
                var zoomUser = _zoomUserRepository.GetAllByEventId(query.EventId);
                var eventDetails = _eventDetailRepository.GetByEvent(query.EventId).FirstOrDefault();
                if (query.EventId != 0)
                {
                    var @event = _eventRepository.Get(query.EventId);
                    eventStatus = @event.EventStatusId == 0 ? @event.IsEnabled ? Contracts.Enums.EventStatus.Published : Contracts.Enums.EventStatus.Draft : @event.EventStatusId;
                    eventName = @event.Name;
                    var eventStepsMapping = _eventStepDetailRepository.GetByEventId(query.EventId);
                    if (eventStepsMapping != null)
                    {
                        completedSteps = eventStepsMapping.CompletedStep;
                        currentStep = _stepProvider.GetCurrentStep(completedSteps);
                    }
                }
                foreach (var step in steps)
                {
                    FIL.Contracts.Models.CreateEventV1.StepModel stepModel = new Contracts.Models.CreateEventV1.StepModel();
                    var currentStepDetail = stepDetails.Where(s => s.StepId == step.Id).FirstOrDefault();
                    stepModel.Name = step.Name;
                    stepModel.Description = currentStepDetail.Description;
                    stepModel.Icon = currentStepDetail.Icon;
                    stepModel.Slug = currentStepDetail.Slug;
                    stepModel.IsEnabled = currentStepDetail.IsEnabled;
                    stepModel.SortOrder = currentStepDetail.SortOrder;
                    stepModel.StepId = currentStepDetail.Id;
                    stepModels.Add(stepModel);
                }
                stepModels = stepModels.OrderBy(s => s.SortOrder).ToList();

                return new StepsQueryResult
                {
                    CompletedStep = completedSteps,
                    EventFrequencyType = eventDetails != null ? eventDetails.EventFrequencyType : Contracts.Enums.EventFrequencyType.None,
                    CurrentStep = currentStep,
                    EventId = query.EventId,
                    StepModel = stepModels,
                    EventName = eventName,
                    EventStatus = eventStatus,
                    IsTransacted = zoomUser.Any() ? true : false,
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new StepsQueryResult { };
            }
        }
    }
}