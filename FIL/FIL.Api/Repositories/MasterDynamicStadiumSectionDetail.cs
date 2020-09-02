using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IMasterDynamicStadiumSectionDetailRepository : IOrmRepository<MasterDynamicStadiumSectionDetail, MasterDynamicStadiumSectionDetail>
    {
        MasterDynamicStadiumSectionDetail Get(int id);
    }

    public class MasterDynamicStadiumSectionDetailRepository : BaseOrmRepository<MasterDynamicStadiumSectionDetail>, IMasterDynamicStadiumSectionDetailRepository
    {
        public MasterDynamicStadiumSectionDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MasterDynamicStadiumSectionDetail Get(int id)
        {
            return Get(new MasterDynamicStadiumSectionDetail { Id = id });
        }

        public IEnumerable<MasterDynamicStadiumSectionDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteMasterDynamicStadiumSectionDetail(MasterDynamicStadiumSectionDetail MasterDynamicStadiumSectionDetail)
        {
            Delete(MasterDynamicStadiumSectionDetail);
        }

        public MasterDynamicStadiumSectionDetail SaveMasterDynamicStadiumSectionDetail(MasterDynamicStadiumSectionDetail MasterDynamicStadiumSectionDetail)
        {
            return Save(MasterDynamicStadiumSectionDetail);
        }
    }
}