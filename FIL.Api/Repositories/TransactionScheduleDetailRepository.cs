using System.Collections.Generic;
using FIL.Api.Core.Utilities;
using FIL.Api.Core.Repositories;
using FIL.Contracts.DataModels;

namespace FIL.Api.Repositories
{
    public interface ITransactionScheduleDetail : IOrmRepository<TransactionScheduleDetail, TransactionScheduleDetail>
    {
        TransactionScheduleDetail Get(long id);
        IEnumerable<TransactionScheduleDetail> GetAllByTransactionDetails(List<long> TransactionDetailIds);
    }

    public class TransactionScheduleDetailRepository : BaseLongOrmRepository<TransactionScheduleDetail>, ITransactionScheduleDetail
    {
        public TransactionScheduleDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TransactionScheduleDetail Get(long id)
        {
            return Get(new TransactionScheduleDetail { Id = id });
        }

        public IEnumerable<TransactionScheduleDetail> GetAllByTransactionDetails(List<long> TransactionDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(TransactionScheduleDetail.TransactionDetailId):C} IN @Id")
               .WithParameters(new { Id = TransactionDetailIds }));
        }
    }
}