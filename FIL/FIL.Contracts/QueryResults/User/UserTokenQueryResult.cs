using System;

namespace FIL.Contracts.QueryResults.User
{
    public class UserTokenQueryResult
    {
        public bool IsValid { get; set; }
        public Guid UserAltId { get; set; }
    }
}