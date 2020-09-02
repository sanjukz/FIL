using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.User;
using System;

namespace FIL.Contracts.Queries.User
{
    public class UserTokenQuery : IQuery<UserTokenQueryResult>
    {
        public Guid AccessToken { get; set; }
    }
}