using FIL.Api.Repositories;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class EventPerformanceQueryHandler : IQueryHandler<EventPerformanceQuery, EventPerformanceQueryResult>
    {
        private readonly ILiveEventDetailRepository _liveEventDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;

        public EventPerformanceQueryHandler(
            ILiveEventDetailRepository liveEventDetailRepository,
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository
            )
        {
            _liveEventDetailRepository = liveEventDetailRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
        }

        public EventPerformanceQueryResult Handle(EventPerformanceQuery query)
        {
            try
            {
                var liveEventDetails = _liveEventDetailRepository.GetByEventId(query.EventId);
                var eventDetails = _eventDetailRepository.GetByEvent(query.EventId).FirstOrDefault();
                var eventData = _eventRepository.Get(eventDetails.EventId);
                FIL.Contracts.Models.CreateEventV1.PerformanceTypeModel performanceTypeModel = new Contracts.Models.CreateEventV1.PerformanceTypeModel();
                if (liveEventDetails == null)
                {
                    return new EventPerformanceQueryResult
                    {
                        EventId = query.EventId,
                        EventAltId = eventData.AltId,
                        EventFrequencyType = eventDetails != null ? eventDetails.EventFrequencyType : Contracts.Enums.EventFrequencyType.None,
                    };
                }
                performanceTypeModel.EventId = liveEventDetails.EventId;
                performanceTypeModel.OnlineEventTypeId = liveEventDetails.OnlineEventTypeId;
                performanceTypeModel.PerformanceTypeId = liveEventDetails.PerformanceTypeId;
                performanceTypeModel.IsEnabled = liveEventDetails.IsEnabled;
                performanceTypeModel.Id = liveEventDetails.Id;
                performanceTypeModel.IsVideoUploaded = liveEventDetails.IsVideoUploaded;
                return new EventPerformanceQueryResult
                {
                    Success = true,
                    EventAltId = eventData.AltId,
                    OnlineEventType = liveEventDetails.OnlineEventTypeId.ToString(),
                    PerformanceTypeModel = performanceTypeModel,
                    EventFrequencyType = eventDetails != null ? eventDetails.EventFrequencyType : Contracts.Enums.EventFrequencyType.None,
                    EventId = query.EventId
                };
            }
            catch (Exception e)
            {
                return new EventPerformanceQueryResult { };
            }
        }
    }
}