using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IUsersWebsiteInvite_InterestRepository : IOrmRepository<UsersWebsiteInvite_Interest, UsersWebsiteInvite_Interest>
    {
        UsersWebsiteInvite_Interest Get(long id);

        UsersWebsiteInvite_Interest GetByEmail(string email);
    }

    public class UsersWebsiteInvite_InterestRepository : BaseLongOrmRepository<UsersWebsiteInvite_Interest>, IUsersWebsiteInvite_InterestRepository
    {
        public UsersWebsiteInvite_InterestRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public UsersWebsiteInvite_Interest Get(long id)
        {
            return Get(id);
        }

        public IEnumerable<UsersWebsiteInvite_Interest> GetAll()
        {
            return GetAll(null);
        }

        public UsersWebsiteInvite_Interest GetByEmail(string email)
        {
            try
            {
                return GetAll(s => s.Where($"{nameof(UsersWebsiteInvite_Interest.Email):C} = @Email")
                .WithParameters(new { Email = email })
                ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}