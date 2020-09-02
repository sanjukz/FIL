using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Events;

namespace FIL.Contracts.Queries.Events
{
    public class EventsQuery : IQuery<EventsQueryResult>
    {
        public Channels Channel { get; set; }
    }
}