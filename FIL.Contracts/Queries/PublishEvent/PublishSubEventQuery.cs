using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.PublishEvent;

namespace FIL.Contracts.Queries.PublishEvent
{
    public class PublishSubEventQuery : IQuery<PublishSubEventQueryResult>
    {
        public long EventId { get; set; }
    }
}