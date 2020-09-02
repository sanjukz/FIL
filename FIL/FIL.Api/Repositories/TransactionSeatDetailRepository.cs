using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ITransactionSeatDetailRepository : IOrmRepository<TransactionSeatDetail, TransactionSeatDetail>
    {
        TransactionSeatDetail Get(long id);

        IEnumerable<TransactionSeatDetail> GetByTransactionDetailIds(IEnumerable<long> transactionDetailIds);

        IEnumerable<TransactionSeatDetail> GetByTransactionDetailId(long transactionDetailIds);
    }

    public class TransactionSeatDetailRepository : BaseLongOrmRepository<TransactionSeatDetail>, ITransactionSeatDetailRepository
    {
        public TransactionSeatDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TransactionSeatDetail Get(long id)
        {
            return Get(new TransactionSeatDetail { Id = id });
        }

        public IEnumerable<TransactionSeatDetail> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<TransactionSeatDetail> GetByTransactionDetailIds(IEnumerable<long> transactionDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(TransactionSeatDetail.TransactionDetailId):C} IN @TransactionDetailIds")
                .WithParameters(new { TransactionDetailIds = transactionDetailIds })
            );
        }

        public IEnumerable<TransactionSeatDetail> GetByTransactionDetailId(long transactionDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(TransactionSeatDetail.TransactionDetailId):C}= @TransactionDetailIds")
                .WithParameters(new { TransactionDetailIds = transactionDetailIds })
            );
        }
    }
}