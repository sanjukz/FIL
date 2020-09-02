using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;
using System;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class GetPahInfoQuery : IQuery<GetPahInfoQueryResult>
    {
        public long TransactionId { get; set; }
        public Guid TransactionAltId { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}