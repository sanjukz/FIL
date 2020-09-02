using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Transaction;
using System;

namespace FIL.Contracts.Queries.Transaction
{
    public class TransactionInfoQuery : IQuery<TransactionInfoQueryResult>
    {
        public Guid TransactionAltId { get; set; }
    }
}