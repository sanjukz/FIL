using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.UsersWebsiteInvite;

namespace FIL.Contracts.Queries
{
    public class UsersWebsiteInviteQuery : IQuery<UsersWebsiteInviteQueryResult>
    {
        public string UserEmail { get; set; }
        public bool IsUsed { get; set; }
    }
}