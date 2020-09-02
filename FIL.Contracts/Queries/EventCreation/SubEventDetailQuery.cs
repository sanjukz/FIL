using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.EventCreation;

namespace FIL.Contracts.Queries.EventCreation
{
    public class SubEventDetailQuery : IQuery<SubEventDetailQueryResult>
    {
        public long EventId { get; set; }
    }
}