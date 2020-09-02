using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface IMasterLayoutCompanionSeatMappingRepository : IOrmRepository<MasterLayoutCompanionSeatMapping, MasterLayoutCompanionSeatMapping>
    {
        MasterLayoutCompanionSeatMapping Get(int id);
    }

    public class MasterLayoutCompanionSeatMappingRepository : BaseOrmRepository<MasterLayoutCompanionSeatMapping>, IMasterLayoutCompanionSeatMappingRepository
    {
        public MasterLayoutCompanionSeatMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public MasterLayoutCompanionSeatMapping Get(int id)
        {
            return Get(new MasterLayoutCompanionSeatMapping { Id = id });
        }
    }
}