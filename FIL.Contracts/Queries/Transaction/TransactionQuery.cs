using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Transaction;

namespace FIL.Contracts.Queries.Transaction
{
    public class TransactionQuery : IQuery<TransactionQueryResult>
    {
        public long? TransactionId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsASI { get; set; }
    }
}