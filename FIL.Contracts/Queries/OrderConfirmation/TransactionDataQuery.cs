using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.OrderConfirmation;

namespace FIL.Contracts.Queries.OrderConfirmation
{
    public class TransactionDataQuery : IQuery<TransactionDataQueryResult>
    {
        public string TransactionIds { get; set; }
    }
}