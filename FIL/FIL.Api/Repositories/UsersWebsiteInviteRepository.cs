using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IUsersWebsiteInviteRepository : IOrmRepository<UsersWebsiteInvite, UsersWebsiteInvite>
    {
        UsersWebsiteInvite Get(long id);

        UsersWebsiteInvite GetById(long id);

        UsersWebsiteInvite GetByEmail(string email);

        UsersWebsiteInvite GetByEmailAndInviteCode(string email, string inviteCode);

        UsersWebsiteInvite GetByInviteCode(string inviteCode);
    }

    public class UsersWebsiteInviteRepository : BaseLongOrmRepository<UsersWebsiteInvite>, IUsersWebsiteInviteRepository
    {
        public UsersWebsiteInviteRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public UsersWebsiteInvite Get(long id)
        {
            return Get(id);
        }

        public UsersWebsiteInvite GetById(long id)
        {
            try
            {
                return GetAll(s => s.Where($"{nameof(UsersWebsiteInvite.Id):C} = @Id")
                .WithParameters(new { Id = id })
                ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<UsersWebsiteInvite> GetAll()
        {
            return GetAll(null);
        }

        public UsersWebsiteInvite GetByEmail(string email)
        {
            try
            {
                return GetAll(s => s.Where($"{nameof(UsersWebsiteInvite.UserEmail):C} = @UserEmail")
                .WithParameters(new { UserEmail = email })
                ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public UsersWebsiteInvite GetByInviteCode(string inviteCode)
        {
            return GetAll(s => s.Where($"{nameof(UsersWebsiteInvite.UserInviteCode):C} = @UserInviteCode")
                .WithParameters(new { UserInviteCode = inviteCode })
            ).FirstOrDefault();
        }

        public UsersWebsiteInvite GetByEmailAndInviteCode(string email, string inviteCode)
        {
            try
            {
                return GetAll(s => s.Where($"{nameof(UsersWebsiteInvite.UserEmail):C}=@UserEmail AND {nameof(UsersWebsiteInvite.UserInviteCode):C}=@UserInviteCode")
                    .WithParameters(new { UserEmail = email, UserInviteCode = inviteCode })
            ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}