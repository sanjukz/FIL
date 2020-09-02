using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITransactionDeliveryDetailRepository : IOrmRepository<TransactionDeliveryDetail, TransactionDeliveryDetail>
    {
        TransactionDeliveryDetail Get(long id);

        TransactionDeliveryDetail GetByTransactionDetailId(long transactionDetailId);

        IEnumerable<TransactionDeliveryDetail> GetByTransactionDetailIds(IEnumerable<long> ids);
    }

    public class TransactionDeliveryDetailRepository : BaseLongOrmRepository<TransactionDeliveryDetail>, ITransactionDeliveryDetailRepository
    {
        public TransactionDeliveryDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TransactionDeliveryDetail Get(long id)
        {
            return Get(new TransactionDeliveryDetail { Id = id });
        }

        public IEnumerable<TransactionDeliveryDetail> GetAll()
        {
            return GetAll(null);
        }

        public TransactionDeliveryDetail GetByTransactionDetailId(long transactionDetailId)
        {
            return GetAll(statement => statement
            .Where($"{nameof(TransactionDeliveryDetail.TransactionDetailId):C} = @TransactionDetailId")
            .WithParameters(new { TransactionDetailId = transactionDetailId })).FirstOrDefault();
        }

        public IEnumerable<TransactionDeliveryDetail> GetByTransactionDetailIds(IEnumerable<long> ids)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(TransactionDeliveryDetail.TransactionDetailId):C} IN @Ids")
                    .WithParameters(new { Ids = ids }));
        }

        public void DeleteTransactionDeliveryDetail(TransactionDeliveryDetail transactionDeliveryDetail)
        {
            Delete(transactionDeliveryDetail);
        }

        public TransactionDeliveryDetail SaveTransactionDeliveryDetail(TransactionDeliveryDetail transactionDeliveryDetail)
        {
            return Save(transactionDeliveryDetail);
        }
    }
}