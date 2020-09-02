using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CurrentOrderData;
using System;

namespace FIL.Contracts.Queries.CurrentOrderData
{
    public class CurrentOrderDataQuery : IQuery<CurrentOrderDataQueryResult>
    {
        public Guid TransactionAltId { get; set; }
    }
}