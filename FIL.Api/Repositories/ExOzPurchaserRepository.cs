using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IExOzPurchaserRepository : IOrmRepository<ExOzPurchaser, ExOzPurchaser>
    {
        ExOzPurchaser Get(int id);
    }

    public class ExOzPurchaserRepository : BaseOrmRepository<ExOzPurchaser>, IExOzPurchaserRepository
    {
        public ExOzPurchaserRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzPurchaser Get(int id)
        {
            return Get(new ExOzPurchaser { Id = id });
        }

        public IEnumerable<ExOzPurchaser> GetAll()
        {
            return GetAll(null);
        }

        public void DeletePurchaser(ExOzPurchaser exOzPurchaser)
        {
            Delete(exOzPurchaser);
        }

        public ExOzPurchaser SavePurchaser(ExOzPurchaser exOzPurchaser)
        {
            return Save(exOzPurchaser);
        }
    }
}