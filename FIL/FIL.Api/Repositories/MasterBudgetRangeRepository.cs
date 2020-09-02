using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface IMasterBudgetRangeRepository : IOrmRepository<MasterBudgetRange, MasterBudgetRange>
    {
        MasterBudgetRange Get(int id);
    }

    public class MasterBudgetRangeRepository : BaseOrmRepository<MasterBudgetRange>, IMasterBudgetRangeRepository
    {
        public MasterBudgetRangeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MasterBudgetRange Get(int id)
        {
            return Get(new MasterBudgetRange { Id = id });
        }
    }
}