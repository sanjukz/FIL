using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IMasterDynamicStadiumCoordinateRepository : IOrmRepository<MasterDynamicStadiumCoordinate, MasterDynamicStadiumCoordinate>
    {
        MasterDynamicStadiumCoordinate Get(int id);
    }

    public class MasterDynamicStadiumCoordinateRepository : BaseOrmRepository<MasterDynamicStadiumCoordinate>, IMasterDynamicStadiumCoordinateRepository
    {
        public MasterDynamicStadiumCoordinateRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MasterDynamicStadiumCoordinate Get(int id)
        {
            return Get(new MasterDynamicStadiumCoordinate { Id = id });
        }

        public IEnumerable<MasterDynamicStadiumCoordinate> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteMasterDynamicStadiumCoordinate(MasterDynamicStadiumCoordinate MasterDynamicStadiumCoordinate)
        {
            Delete(MasterDynamicStadiumCoordinate);
        }

        public MasterDynamicStadiumCoordinate SaveMasterDynamicStadiumCoordinate(MasterDynamicStadiumCoordinate MasterDynamicStadiumCoordinate)
        {
            return Save(MasterDynamicStadiumCoordinate);
        }
    }
}