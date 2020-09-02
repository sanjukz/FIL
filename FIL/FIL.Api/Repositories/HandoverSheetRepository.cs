using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface IHandoverSheetRepository : IOrmRepository<HandoverSheet, HandoverSheet>
    {
        HandoverSheet Get(long id);
    }

    public class HandoverSheetRepository : BaseLongOrmRepository<HandoverSheet>, IHandoverSheetRepository
    {
        public HandoverSheetRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public HandoverSheet Get(long id)
        {
            return Get(new HandoverSheet { Id = id });
        }
    }
}