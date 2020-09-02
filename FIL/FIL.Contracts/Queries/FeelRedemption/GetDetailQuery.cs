using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.FeelRedemption;
using System;

namespace FIL.Contracts.Queries.FeelRedemption
{
    public class GetDetailQuery : IQuery<GetDetailQueryResult>
    {
        public long TransactionId { get; set; }
        public Guid UserAltId { get; set; }
    }
}