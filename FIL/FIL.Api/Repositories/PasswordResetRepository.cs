using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IPasswordResetRepository : IOrmRepository<PasswordToken, PasswordToken>
    {
        PasswordToken Get(long id);
    }

    public class PasswordResetRepository : BaseLongOrmRepository<PasswordToken>, IPasswordResetRepository
    {
        public PasswordResetRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public PasswordToken Get(long id)
        {
            return Get(new PasswordToken { Id = id });
        }

        public IEnumerable<PasswordToken> GetAll()
        {
            return GetAll(null);
        }
    }
}