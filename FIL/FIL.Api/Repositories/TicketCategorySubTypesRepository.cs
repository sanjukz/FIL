using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface ITicketCategorySubTypesRepository : IOrmRepository<TicketCategorySubType, TicketCategorySubType>
    {
        TicketCategorySubType Get(long id);
    }

    public class TicketCategorySubTypesRepository : BaseLongOrmRepository<TicketCategorySubType>, ITicketCategorySubTypesRepository
    {
        public TicketCategorySubTypesRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public TicketCategorySubType Get(long id)
        {
            return Get(new TicketCategorySubType { Id = id });
        }
    }
}