using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ITransactionPromoCodeRepository : IOrmRepository<TransactionPromoCode, TransactionPromoCode>
    {
        TransactionPromoCode Get(int id);
    }

    public class TransactionPromoCodeRepository : BaseOrmRepository<TransactionPromoCode>, ITransactionPromoCodeRepository
    {
        public TransactionPromoCodeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TransactionPromoCode Get(int id)
        {
            return Get(new TransactionPromoCode { Id = id });
        }

        public IEnumerable<TransactionPromoCode> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteTransactionPromoCode(TransactionPromoCode transactionPromoCode)
        {
            Delete(transactionPromoCode);
        }

        public TransactionPromoCode SaveTransactionPromoCode(TransactionPromoCode transactionPromoCode)
        {
            return Save(transactionPromoCode);
        }
    }
}