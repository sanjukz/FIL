using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.POne;

namespace FIL.Api.Repositories
{
    public interface IPOneBookingRepository : IOrmRepository<POneTransactionDetail, POneTransactionDetail>
    {
        POneTransactionDetail Get(int id);
    }

    public class POneBookingRepository : BaseOrmRepository<POneTransactionDetail>, IPOneBookingRepository
    {
        public POneBookingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public POneTransactionDetail Get(int id)
        {
            return Get(new POneTransactionDetail { Id = id });
        }
    }
}