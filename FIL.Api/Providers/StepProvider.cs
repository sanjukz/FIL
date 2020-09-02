using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers
{
    public interface IStepProvider
    {
        EventStepDetail SaveEventStepDetails(long EventId,
            short stepId,
            bool IsAppend,
            Guid ModifiedBy);

        short GetCurrentStep(string CompletedStep);
    }

    public class StepProvider : IStepProvider
    {
        private readonly FIL.Logging.ILogger _logger;
        private readonly IEventStepDetailRepository _eventStepDetailRepository;
        private readonly IStepDetailsRepository _stepDetailsRepository;

        public StepProvider(
            FIL.Logging.ILogger logger,
            IStepDetailsRepository stepDetailsRepository,
            IEventStepDetailRepository eventStepDetailRepository)
        {
            _logger = logger;
            _eventStepDetailRepository = eventStepDetailRepository;
            _stepDetailsRepository = stepDetailsRepository;
        }

        public short GetCurrentStep(string CompletedStep)
        {
            List<int> completedStepList = CompletedStep.Split(',').Select(int.Parse).OrderByDescending(s => s).ToList();
            var stepDetails = _stepDetailsRepository.GetAll().Select(s => s.MasterEventTypeId == Contracts.Enums.MasterEventType.Online && s.IsEnabled);
            if (completedStepList.Count() == stepDetails.Count())
            {
                return 1;
            }
            else
            {
                return (short)Enumerable.Range(1, stepDetails.Count()).Except(completedStepList).FirstOrDefault();
            }
        }

        public EventStepDetail SaveEventStepDetails(
            long EventId,
            short stepId,
            bool IsAppend,
            Guid ModifiedBy)
        {
            try
            {
                if (stepId == 0)
                {
                    return new EventStepDetail { };
                }
                FIL.Contracts.DataModels.EventStepDetail eventStepDetail = new EventStepDetail();
                var eventStepDetail1 = _eventStepDetailRepository.GetByEventId(EventId);
                eventStepDetail.CreatedBy = eventStepDetail1 == null ? ModifiedBy : eventStepDetail1.CreatedBy;
                eventStepDetail.UpdatedBy = ModifiedBy;
                eventStepDetail.ModifiedBy = ModifiedBy;
                eventStepDetail.CreatedUtc = eventStepDetail1 == null ? DateTime.UtcNow : eventStepDetail1.CreatedUtc;
                eventStepDetail.UpdatedUtc = DateTime.UtcNow;
                eventStepDetail.CompletedStep = eventStepDetail1 != null ? eventStepDetail1.CompletedStep : null;
                var currentStepId = stepId.ToString();
                if (eventStepDetail1 == null)
                {
                    eventStepDetail.CompletedStep = currentStepId;
                }
                else
                {
                    if (!eventStepDetail1.CompletedStep.Contains(currentStepId) && IsAppend)
                    {
                        eventStepDetail.CompletedStep = eventStepDetail1.CompletedStep + "," + currentStepId;
                    }
                    else if (!IsAppend)
                    {
                        eventStepDetail.CompletedStep = eventStepDetail1.CompletedStep.Replace("," + currentStepId, "");
                    }
                }
                eventStepDetail.CompletedStep = eventStepDetail.CompletedStep.Trim(',');
                eventStepDetail.EventId = EventId;
                eventStepDetail.IsEnabled = true;
                eventStepDetail.Id = eventStepDetail1 == null ? 0 : eventStepDetail1.Id;
                eventStepDetail.CurrentStep = GetCurrentStep(eventStepDetail.CompletedStep);
                return _eventStepDetailRepository.Save(eventStepDetail);
            }
            catch (Exception e)
            {
                _logger.Log(FIL.Logging.Enums.LogCategory.Error, e);
                return new EventStepDetail { };
            }
        }
    }
}