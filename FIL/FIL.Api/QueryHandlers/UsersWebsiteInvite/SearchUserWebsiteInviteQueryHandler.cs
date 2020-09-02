using FIL.Api.Repositories;
using FIL.Contracts.Queries;
using FIL.Contracts.QueryResults.UsersWebsiteInvite;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.UsersWebsiteInvite
{
    public class SearchUserWebsiteInviteQueryHandler : IQueryHandler<SearchUserWebsiteInviteQuery, UsersWebsiteInviteQueryResult>
    {
        private readonly IUsersWebsiteInviteRepository _usersWebsiteInviteRepository;

        public SearchUserWebsiteInviteQueryHandler(IUsersWebsiteInviteRepository usersWebsiteInviteRepository)
        {
            _usersWebsiteInviteRepository = usersWebsiteInviteRepository;
        }

        public UsersWebsiteInviteQueryResult Handle(SearchUserWebsiteInviteQuery query)
        {
            IEnumerable<FIL.Contracts.Models.UsersWebsiteInvite> UsersWebsiteInvite = new List<FIL.Contracts.Models.UsersWebsiteInvite>();
            if (string.IsNullOrWhiteSpace(query.SearchString))
            {
                UsersWebsiteInvite = _usersWebsiteInviteRepository.GetAll().Where(x => x.IsUsed == query.IsUsed);
            }
            else if (!string.IsNullOrWhiteSpace(query.SearchString))
            {
                UsersWebsiteInvite = _usersWebsiteInviteRepository.GetAll().Where(x => (x.UserEmail.ToUpper().Contains(query.SearchString.ToUpper())) && x.IsUsed == query.IsUsed);
            }

            if (UsersWebsiteInvite == null)
            {
                return new UsersWebsiteInviteQueryResult();
            }
            else
            {
                return new UsersWebsiteInviteQueryResult()
                {
                    UsersWebsiteInvite = UsersWebsiteInvite.ToList()
                };
            }
        }
    }
}