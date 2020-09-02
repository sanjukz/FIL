using FIL.Api.Repositories;
using FIL.Contracts.Queries.Events;
using FIL.Contracts.QueryResults.Events;
using System.Linq;

namespace FIL.Api.QueryHandlers.Events
{
    public class EventSiteIdMappingQueryHandler : IQueryHandler<EventSiteIdMappingQuery, EventSiteIdMappingQueryResult>
    {
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;

        public EventSiteIdMappingQueryHandler(IEventSiteIdMappingRepository eventSiteIdMappingRepository)
        {
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
        }

        public EventSiteIdMappingQueryResult Handle(EventSiteIdMappingQuery query)
        {
            var sitemapResult = _eventSiteIdMappingRepository.GetAllByEventId(query.EventId);
            return new EventSiteIdMappingQueryResult() { EventSiteIdMappings = sitemapResult.ToList() };
        }
    }
}