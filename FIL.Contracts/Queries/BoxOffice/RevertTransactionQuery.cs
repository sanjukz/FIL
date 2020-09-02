using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class RevertTransactionQuery : IQuery<RevertTransactionQueryResult>
    {
        public long TransactionId { get; set; }
    }
}