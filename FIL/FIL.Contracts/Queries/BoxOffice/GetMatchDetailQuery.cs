using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class GetMatchDetailQuery : IQuery<GetMatchDetailQueryResult>
    {
        public long TransactionId { get; set; }
    }
}