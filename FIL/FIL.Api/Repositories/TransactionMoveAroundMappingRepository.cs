using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITransactionMoveAroundMappingRepository : IOrmRepository<TransactionMoveAroundMapping, TransactionMoveAroundMapping>
    {
        TransactionMoveAroundMapping Get(int id);

        TransactionMoveAroundMapping GetByTransactionId(long transactionId);
    }

    public class TransactionMoveAroundMappingRepository : BaseOrmRepository<TransactionMoveAroundMapping>, ITransactionMoveAroundMappingRepository
    {
        public TransactionMoveAroundMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TransactionMoveAroundMapping Get(int id)
        {
            return Get(new TransactionMoveAroundMapping { Id = id });
        }

        public TransactionMoveAroundMapping GetByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionMoveAroundMapping.TransactionId):C} = @TransactionIdParam").WithParameters(new { TransactionIdParam = transactionId })).FirstOrDefault();
        }

        public IEnumerable<TransactionMoveAroundMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteValueRetailVillage(TransactionMoveAroundMapping transactionMoveAroundMapping)
        {
            Delete(transactionMoveAroundMapping);
        }

        public TransactionMoveAroundMapping SaveValueRetailVillage(TransactionMoveAroundMapping transactionMoveAroundMapping)
        {
            return Save(transactionMoveAroundMapping);
        }
    }
}