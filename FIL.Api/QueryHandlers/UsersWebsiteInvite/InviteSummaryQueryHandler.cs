using FIL.Api.Repositories;
using FIL.Contracts.Queries.WebsiteInvite;
using FIL.Contracts.QueryResults.UsersWebsiteInvite;
using System.Linq;

namespace FIL.Api.QueryHandlers.UsersWebsiteInvite
{
    public class InviteSummaryQueryHandler : IQueryHandler<InviteSummaryQuery, InviteSummaryQueryResult>
    {
        private readonly IUsersWebsiteInviteRepository _usersWebsiteInviteRepository;

        public InviteSummaryQueryHandler(IUsersWebsiteInviteRepository usersWebsiteInviteRepository)
        {
            _usersWebsiteInviteRepository = usersWebsiteInviteRepository;
        }

        public InviteSummaryQueryResult Handle(InviteSummaryQuery query)
        {
            return new InviteSummaryQueryResult
            {
                UsedMails = _usersWebsiteInviteRepository.GetAll().Count(x => x.IsUsed == true),
                UnUsedMails = _usersWebsiteInviteRepository.GetAll().Count(x => x.IsUsed == false),
                TotalMails = _usersWebsiteInviteRepository.GetAll().Count()
            };
        }
    }
}