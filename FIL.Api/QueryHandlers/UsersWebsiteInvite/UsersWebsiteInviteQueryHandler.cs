using FIL.Api.Repositories;
using FIL.Contracts.Queries;
using FIL.Contracts.QueryResults.UsersWebsiteInvite;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.UsersWebsiteInvite
{
    public class UsersWebsiteInviteQueryHandler : IQueryHandler<UsersWebsiteInviteQuery, UsersWebsiteInviteQueryResult>
    {
        private readonly IUsersWebsiteInviteRepository _usersWebsiteInviteRepository;

        public UsersWebsiteInviteQueryHandler(IUsersWebsiteInviteRepository usersWebsiteInviteRepository)
        {
            _usersWebsiteInviteRepository = usersWebsiteInviteRepository;
        }

        public UsersWebsiteInviteQueryResult Handle(UsersWebsiteInviteQuery query)
        {
            List<FIL.Contracts.Models.UsersWebsiteInvite> UsersWebsiteInvite = new List<FIL.Contracts.Models.UsersWebsiteInvite>();

            var userwebsiteinvite = _usersWebsiteInviteRepository.GetAll();
            if (userwebsiteinvite == null)
            {
                return new UsersWebsiteInviteQueryResult();
            }
            else
            {
                return new UsersWebsiteInviteQueryResult()
                {
                    UsersWebsiteInvite = userwebsiteinvite.ToList()
                };
            }
        }
    }
}