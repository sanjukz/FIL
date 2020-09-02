using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.EventCreation;

namespace FIL.Contracts.Queries.EventCreation
{
    public class GetticketDetailQuery : IQuery<GetticketDetailQueryResult>
    {
        public long EventDetailId { get; set; }
    }
}