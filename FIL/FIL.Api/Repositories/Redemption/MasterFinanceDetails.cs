using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Redemption;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories.Redemption
{
    public interface IMasterFinanceDetailsRepository : IOrmRepository<MasterFinanceDetails, MasterFinanceDetails>
    {
        MasterFinanceDetails Get(int Id);

        MasterFinanceDetails GetByEventId(long eventId);
    }

    public class MasterFinanceDetailsRepository : BaseOrmRepository<MasterFinanceDetails>, IMasterFinanceDetailsRepository
    {
        public MasterFinanceDetailsRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MasterFinanceDetails Get(int Id)
        {
            return Get(new MasterFinanceDetails { Id = Id });
        }

        public MasterFinanceDetails GetByEventId(long Id)
        {
            return GetAll(s => s.Where($"{nameof(MasterFinanceDetails.EventId):C} = @EventId")
               .WithParameters(new { EventId = Id })
           ).FirstOrDefault();
        }

        public IEnumerable<MasterFinanceDetails> GetAll()
        {
            return GetAll(null);
        }
    }
}