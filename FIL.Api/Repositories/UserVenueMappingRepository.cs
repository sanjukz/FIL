using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IUserVenueMappingRepository : IOrmRepository<UserVenueMapping, UserVenueMapping>
    {
        UserVenueMapping Get(int id);

        IEnumerable<UserVenueMapping> GetByUserId(long userId);
    }

    public class UserVenueMappingRepository : BaseOrmRepository<UserVenueMapping>, IUserVenueMappingRepository
    {
        public UserVenueMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public UserVenueMapping Get(int id)
        {
            return Get(new UserVenueMapping { Id = id });
        }

        public IEnumerable<UserVenueMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteUserVenueMapping(UserVenueMapping UserVenueMapping)
        {
            Delete(UserVenueMapping);
        }

        public UserVenueMapping SaveUserVenueMapping(UserVenueMapping UserVenueMapping)
        {
            return Save(UserVenueMapping);
        }

        public IEnumerable<UserVenueMapping> GetByUserId(long userId)
        {
            return GetAll(statement => statement
              .Where($"{nameof(UserVenueMapping.UserId):C} = @UserId AND IsEnabled=1")
              .WithParameters(new { UserId = userId }));
        }
    }
}