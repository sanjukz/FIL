using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface ICorporateTicketAllocationDetailLogRepository : IOrmRepository<CorporateTicketAllocationDetailLog, CorporateTicketAllocationDetailLog>
    {
        CorporateTicketAllocationDetail Get(long id);
    }

    public class CorporateTicketAllocationDetailLogRepository : BaseLongOrmRepository<CorporateTicketAllocationDetailLog>, ICorporateTicketAllocationDetailLogRepository
    {
        public CorporateTicketAllocationDetailLogRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CorporateTicketAllocationDetail Get(long id)
        {
            return (new CorporateTicketAllocationDetail { Id = id });
        }
    }
}