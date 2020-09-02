using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface ITicketCategoryDetailRepository : IOrmRepository<TicketCategoryDetail, TicketCategoryDetail>
    {
        TicketCategoryDetail Get(int id);
    }

    public class TicketCategoryDetailRepository : BaseLongOrmRepository<TicketCategoryDetail>, ITicketCategoryDetailRepository
    {
        public TicketCategoryDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TicketCategoryDetail Get(int id)
        {
            return Get(new TicketCategoryDetail { Id = id });
        }
    }
}