using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IFeelUserAdditionalDetailRepository : IOrmRepository<FeelUserAdditionalDetail, FeelUserAdditionalDetail>
    {
        FeelUserAdditionalDetail Get(int id);

        FeelUserAdditionalDetail GetByUserId(int userId);
        IEnumerable<FeelUserAdditionalDetail> GetByUserIds(List<long> userId);
    }

    public class FeelUserAdditionalDetailRepository : BaseOrmRepository<FeelUserAdditionalDetail>, IFeelUserAdditionalDetailRepository
    {
        public FeelUserAdditionalDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public FeelUserAdditionalDetail Get(int id)
        {
            return Get(new FeelUserAdditionalDetail { Id = id });
        }

        public FeelUserAdditionalDetail GetByUserId(int userId)
        {
            return GetAll(s => s.Where($"{nameof(FeelUserAdditionalDetail.UserId):C} = @UserId")
                .WithParameters(new { UserId = userId })
            ).LastOrDefault();
        }

        public IEnumerable<FeelUserAdditionalDetail> GetByUserIds(List<long> userId)
        {
            return GetAll(s => s.Where($"{nameof(FeelUserAdditionalDetail.UserId):C} IN @UserId")
                .WithParameters(new { UserId = userId })
            );
        }

        public IEnumerable<FeelUserAdditionalDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteUser(FeelUserAdditionalDetail user)
        {
            Delete(user);
        }

        public FeelUserAdditionalDetail SaveUser(FeelUserAdditionalDetail user)
        {
            return Save(user);
        }
    }
}