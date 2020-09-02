using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Redemption;
using System;

namespace FIL.Contracts.Queries.Redemption
{
    public class TokenQuery : IQuery<TokenQueryResult>
    {
        public Guid TokenId { get; set; }
    }
}