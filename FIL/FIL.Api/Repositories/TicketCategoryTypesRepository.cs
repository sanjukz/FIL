using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface ITicketCategoryTypesRepository : IOrmRepository<TicketCategoryType, TicketCategoryType>
    {
        TicketCategoryType Get(long id);
    }

    public class TicketCategoryTypesRepository : BaseLongOrmRepository<TicketCategoryType>, ITicketCategoryTypesRepository
    {
        public TicketCategoryTypesRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public TicketCategoryType Get(long id)
        {
            return Get(new TicketCategoryType { Id = id });
        }
    }
}