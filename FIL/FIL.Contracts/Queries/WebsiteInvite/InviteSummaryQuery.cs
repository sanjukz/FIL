using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.UsersWebsiteInvite;

namespace FIL.Contracts.Queries.WebsiteInvite
{
    public class InviteSummaryQuery : IQuery<InviteSummaryQueryResult>
    {
        public bool ISUsed { get; set; }
    }
}