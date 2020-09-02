using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IRefillWalletDetailRepository : IOrmRepository<RefillWalletDetail, RefillWalletDetail>
    {
        RefillWalletDetail Get(int id);
    }

    public class RefillWalletDetailRepository : BaseOrmRepository<RefillWalletDetail>, IRefillWalletDetailRepository
    {
        public RefillWalletDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public RefillWalletDetail Get(int id)
        {
            return Get(new RefillWalletDetail { Id = id });
        }

        public IEnumerable<RefillWalletDetail> GetAll()
        {
            return GetAll(null);
        }
    }
}