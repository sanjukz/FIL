using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.EventAttendee;

namespace FIL.Contracts.Queries.EventAttendee
{
    public class EventAttendeeQuery : IQuery<EventAttendeeQueryResult>
    {
        public long? TransactionId { get; set; }
    }
}