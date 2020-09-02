using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;

namespace FIL.Contracts.Queries.Events
{
    public class EventCategoryMappingQuery : IQuery<EventCategoryMappingQueryResult>
    {
        public int EventId { get; set; }
    }
}