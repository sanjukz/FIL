using FIL.Api.Repositories;
using FIL.Contracts.Queries.Events;
using FIL.Contracts.QueryResults.Events;
using System.Linq;

namespace FIL.Api.QueryHandlers.Events
{
    public class EventSiteIdMappingSortQueryHandler : IQueryHandler<EventSiteIdMappingSortQuery, EventSiteIdMappingQueryResult>
    {
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;

        public EventSiteIdMappingSortQueryHandler(IEventSiteIdMappingRepository eventSiteIdMappingRepository)
        {
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
        }

        public EventSiteIdMappingQueryResult Handle(EventSiteIdMappingSortQuery query)
        {
            var sitemapResult = _eventSiteIdMappingRepository.GetBySiteId(query.SiteId).OrderBy(x => x.SortOrder);
            return new EventSiteIdMappingQueryResult() { EventSiteIdMappings = sitemapResult.ToList() };
        }
    }
}