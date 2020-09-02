using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.UserCard;
using System;

namespace FIL.Contracts.Queries.UserCard
{
    public class UserCardQuery : IQuery<UserCardQueryResult>
    {
        public Guid UserAltId { get; set; }
    }
}