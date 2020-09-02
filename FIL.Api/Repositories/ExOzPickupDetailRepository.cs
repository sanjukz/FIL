using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IExOzPickupRepository : IOrmRepository<ExOzPickup, ExOzPickup>
    {
        ExOzPickup Get(int id);
    }

    public class ExOzPickupRepository : BaseOrmRepository<ExOzPickup>, IExOzPickupRepository
    {
        public ExOzPickupRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzPickup Get(int id)
        {
            return Get(new ExOzPickup { Id = id });
        }

        public IEnumerable<ExOzPickup> GetAll()
        {
            return GetAll(null);
        }

        public void DeletePickup(ExOzPickup ExOzPickup)
        {
            Delete(ExOzPickup);
        }

        public ExOzPickup SavePickup(ExOzPickup ExOzPickup)
        {
            return Save(ExOzPickup);
        }
    }
}