using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;

namespace FIL.Contracts.Queries.Events
{
    public class EventSiteIdMappingQuery : IQuery<EventSiteIdMappingQueryResult>
    {
        public int EventId { get; set; }
    }
}