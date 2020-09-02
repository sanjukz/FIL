using FIL.Api.Repositories;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class EventHostQueryHandler : IQueryHandler<EventHostQuery, EventHostQueryResult>
    {
        private readonly IEventHostMappingRepository _eventHostMappingRepository;
        private readonly IEventRepository _eventRepository;

        public EventHostQueryHandler(
            IEventHostMappingRepository eventHostMappingRepository,
            IEventRepository eventRepository
            )
        {
            _eventHostMappingRepository = eventHostMappingRepository;
            _eventRepository = eventRepository;
        }

        public EventHostQueryResult Handle(EventHostQuery query)
        {
            try
            {
                var eventData = _eventRepository.Get(query.EventId);
                if (eventData == null)
                {
                    return new EventHostQueryResult { Success = true };
                }
                var eventHostMappings = _eventHostMappingRepository.GetAllByEventId(query.EventId).ToList();
                return new EventHostQueryResult
                {
                    Success = true,
                    EventId = query.EventId,
                    IsValidLink = true,
                    IsDraft = false,
                    EventHostMapping = eventHostMappings
                };
            }
            catch (Exception e)
            {
                return new EventHostQueryResult { };
            }
        }
    }
}