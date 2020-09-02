using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IUserTokenMappingRepository : IOrmRepository<UserTokenMapping, UserTokenMapping>
    {
        UserTokenMapping Get(int id);

        UserTokenMapping GetByTokenId(int tokenId);
    }

    public class UserTokenMappingRepository : BaseOrmRepository<UserTokenMapping>, IUserTokenMappingRepository
    {
        public UserTokenMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public UserTokenMapping Get(int id)
        {
            return Get(new UserTokenMapping { Id = id });
        }

        public IEnumerable<UserTokenMapping> GetAll()
        {
            return GetAll(null);
        }

        public UserTokenMapping GetByTokenId(int tokenId)
        {
            return GetAll(s => s.Where($"{nameof(UserTokenMapping.TokenId):C} = @TokenId")
                .WithParameters(new { TokenId = tokenId })
            ).FirstOrDefault();
        }
    }
}