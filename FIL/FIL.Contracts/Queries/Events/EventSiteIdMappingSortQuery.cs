using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;

namespace FIL.Contracts.Queries.Events
{
    public class EventSiteIdMappingSortQuery : IQuery<EventSiteIdMappingQueryResult>
    {
        public Site SiteId { get; set; }
    }
}