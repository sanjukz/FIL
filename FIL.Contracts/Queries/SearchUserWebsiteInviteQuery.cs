using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.UsersWebsiteInvite;

namespace FIL.Contracts.Queries
{
    public class SearchUserWebsiteInviteQuery : IQuery<UsersWebsiteInviteQueryResult>
    {
        public string SearchString { get; set; }
        public bool IsUsed { get; set; }
    }
}