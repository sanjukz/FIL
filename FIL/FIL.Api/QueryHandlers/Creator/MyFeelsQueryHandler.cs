using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Queries.Creator;
using FIL.Contracts.QueryResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Creator
{
    public class MyFeelsQueryHandler : IQueryHandler<MyFeelsQuery, MyFeelsQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IStepRepository _stepRepository;
        private readonly IStepDetailsRepository _stepDetailsRepository;
        public readonly ILocalTimeZoneConvertProvider _localTimeZoneConvertProvider;
        private readonly FIL.Logging.ILogger _logger;

        public MyFeelsQueryHandler(
            IEventRepository eventRepository,
            IStepRepository stepRepository,
            IStepDetailsRepository stepDetailsRepository,
            ILocalTimeZoneConvertProvider localTimeZoneConvertProvider,
            FIL.Logging.ILogger logger)
        {
            _stepDetailsRepository = stepDetailsRepository;
            _eventRepository = eventRepository;
            _stepRepository = stepRepository;
            _localTimeZoneConvertProvider = localTimeZoneConvertProvider;
            _logger = logger;
        }

        public short GetCurrentTab(List<FIL.Contracts.DataModels.StepDetail> stepDetails, string completedStep)
        {
            List<int> completedStepList = completedStep.Split(',').Select(int.Parse).OrderByDescending(s => s).ToList();
            return (short)stepDetails.Select(s => s.Id).Except(completedStepList).FirstOrDefault();
        }

        public MyFeelsQueryResult Handle(MyFeelsQuery query)
        {
            try
            {
                var events = new List<FIL.Contracts.Models.Creator.MyFeel>();
                if (query.IsApproveModerateScreen)
                {
                    events = _eventRepository.GetApproveModerateFeels(query.IsActive).Take(500).ToList();
                }
                else
                {
                    events = _eventRepository.GetMyFeels(query.CreatedBy).Take(500).ToList();
                }
                var steps = _stepRepository.GetAll().ToList();
                var stepDetails = _stepDetailsRepository.GetAll().Where(s => s.MasterEventTypeId == Contracts.Enums.MasterEventType.Online && s.IsEnabled).OrderBy(s => s.Id).ToList();
                foreach (var currentFeel in events)
                {
                    if (currentFeel.EventEndDateTime < DateTime.UtcNow)
                    {
                        currentFeel.IsPastEvent = true;
                    }
                    if (!String.IsNullOrEmpty(currentFeel.TimeZoneOffset))
                    {
                        currentFeel.EventStartDateTime = _localTimeZoneConvertProvider.ConvertToLocal(currentFeel.EventStartDateTime, currentFeel.TimeZoneOffset);
                        currentFeel.EventEndDateTime = _localTimeZoneConvertProvider.ConvertToLocal(currentFeel.EventEndDateTime, currentFeel.TimeZoneOffset);
                    }
                    currentFeel.EventStartDateTimeString = currentFeel.EventStartDateTime.ToString("MMM dd, yyyy HH:mm").ToUpper();
                    currentFeel.EventEndDateTimeString = currentFeel.EventEndDateTime.ToString("MMM dd, yyyy HH:mm").ToUpper();
                    if (currentFeel.CompletedStep != null && !query.IsApproveModerateScreen)
                    {
                        var currentStep = GetCurrentTab(stepDetails, currentFeel.CompletedStep);
                        if (currentStep != 0)
                        {
                            currentFeel.CurrentStep = steps.Where(s => s.Id == stepDetails.Where(p => p.Id == currentStep).FirstOrDefault().StepId).FirstOrDefault().Name;
                            currentFeel.IsShowExclamationIcon = true;
                        }
                    }
                }
                return new MyFeelsQueryResult
                {
                    MyFeels = events,
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new MyFeelsQueryResult
                {
                    Success = true
                };
            }
        }
    }
}