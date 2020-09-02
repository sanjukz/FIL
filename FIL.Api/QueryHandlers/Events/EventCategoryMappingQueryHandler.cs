using FIL.Api.Repositories;
using FIL.Contracts.Queries.Events;
using FIL.Contracts.QueryResults.Events;
using System.Linq;

namespace FIL.Api.QueryHandlers.Events
{
    public class EventCategoryMappingQueryHandler : IQueryHandler<EventCategoryMappingQuery, EventCategoryMappingQueryResult>
    {
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;

        public EventCategoryMappingQueryHandler(IEventCategoryMappingRepository eventCategoryMappingRepository)
        {
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
        }

        public EventCategoryMappingQueryResult Handle(EventCategoryMappingQuery query)
        {
            var catmapResult = _eventCategoryMappingRepository.GetByEventId(query.EventId);
            return new EventCategoryMappingQueryResult() { EventCategoryMappings = catmapResult.ToList() };
        }
    }
}