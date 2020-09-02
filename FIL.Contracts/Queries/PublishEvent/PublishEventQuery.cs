using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.PublishEvent;

namespace FIL.Contracts.Queries.PublishEvent
{
    public class PublishEventQuery : IQuery<PublishEventQueryResult>
    {
        public long EventId { get; set; }
    }
}