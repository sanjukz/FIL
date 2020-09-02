using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Tiqets;
using System.Linq;

namespace FIL.Api.Repositories.Tiqets
{
    public interface ITiqetsTransactionRepository : IOrmRepository<TiqetsTransaction, TiqetsTransaction>
    {
        TiqetsTransaction Get(long id);

        TiqetsTransaction GetByTransactionId(long transactionId);
    }

    public class TiqetsTransactionRepository : BaseLongOrmRepository<TiqetsTransaction>, ITiqetsTransactionRepository
    {
        public TiqetsTransactionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TiqetsTransaction Get(long id)
        {
            return Get(new TiqetsTransaction { Id = id });
        }

        public TiqetsTransaction GetByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(TiqetsTransaction.TransactionId):C}=@TransactionId ")
            .WithParameters(new { TransactionId = transactionId })).FirstOrDefault();
        }
    }
}