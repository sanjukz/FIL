using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CreateEventV1;

namespace FIL.Contracts.Queries.CreateEventV1
{
    public class EventHostQuery : IQuery<EventHostQueryResult>
    {
        public long EventId { get; set; }
    }
}