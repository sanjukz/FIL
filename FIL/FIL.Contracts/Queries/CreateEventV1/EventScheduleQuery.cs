using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CreateEventV1;

namespace FIL.Contracts.Queries.CreateEventV1
{
    public class EventScheduleQuery : IQuery<EventScheduleQueryResult>
    {
        public long EventId { get; set; }
    }
}