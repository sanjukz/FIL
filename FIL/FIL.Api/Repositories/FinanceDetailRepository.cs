using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IFinanceDetailRepository : IOrmRepository<FinanceDetail, FinanceDetail>
    {
        FinanceDetail GetFinanceDetailsByEventId(long EventId);
    }

    public class FinanceDetailRepository : BaseOrmRepository<FinanceDetail>, IFinanceDetailRepository
    {
        public FinanceDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public FinanceDetail GetFinanceDetailsByEventId(long EventId)
        {
            return GetAll(s => s.Where($"{nameof(FinanceDetail.EventId):C} = @EventId")
                .WithParameters(new { EventId = EventId })
            ).FirstOrDefault();
        }
    }
}