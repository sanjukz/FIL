using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class ReprintRequestQuery : IQuery<ReprintRequestQueryResult>
    {
        public long TransactionId { get; set; }
    }
}