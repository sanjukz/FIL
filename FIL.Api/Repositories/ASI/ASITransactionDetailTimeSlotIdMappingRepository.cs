using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IASITransactionDetailTimeSlotIdMappingRepository : IOrmRepository<ASITransactionDetailTimeSlotIdMapping, ASITransactionDetailTimeSlotIdMapping>
    {
        ASITransactionDetailTimeSlotIdMapping Get(long id);

        ASITransactionDetailTimeSlotIdMapping GetByTransactionDetailId(long id);

        IEnumerable<ASITransactionDetailTimeSlotIdMapping> GetByTransactionDetailIds(IEnumerable<long> id);
    }

    public class ASITransactionDetailTimeSlotIdMappingRepository : BaseLongOrmRepository<ASITransactionDetailTimeSlotIdMapping>, IASITransactionDetailTimeSlotIdMappingRepository
    {
        public ASITransactionDetailTimeSlotIdMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public ASITransactionDetailTimeSlotIdMapping Get(long id)
        {
            return Get(new ASITransactionDetailTimeSlotIdMapping { Id = id });
        }

        public ASITransactionDetailTimeSlotIdMapping GetByTransactionDetailId(long id)
        {
            return GetAll(s => s.Where($"{nameof(ASITransactionDetailTimeSlotIdMapping.TransactionDetailId):C} = @TransactionDetailId")
                .WithParameters(new { TransactionDetailId = id })
            ).FirstOrDefault();
        }

        public IEnumerable<ASITransactionDetailTimeSlotIdMapping> GetByTransactionDetailIds(IEnumerable<long> ids)
        {
            return GetAll(s => s.Where($"{nameof(ASITransactionDetailTimeSlotIdMapping.TransactionDetailId):C} IN @TransactionDetailId")
                .WithParameters(new { TransactionDetailId = ids })
            );
        }
    }
}