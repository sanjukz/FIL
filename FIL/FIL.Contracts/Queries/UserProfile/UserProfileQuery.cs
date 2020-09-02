using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.UserProfile;
using System;

namespace FIL.Contracts.Queries.UserProfile
{
    public class UserProfileQuery : IQuery<UserProfileQueryResult>
    {
        public Guid AltId { get; set; }
    }
}