using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITransactionReleaseLogRepository : IOrmRepository<TransactionReleaseLog, TransactionReleaseLog>
    {
        TransactionReleaseLog Get(long id);

        TransactionReleaseLog GetByTransactionId(long transactionId);
    }

    public class TransactionReleaseLogRepository : BaseLongOrmRepository<TransactionReleaseLog>, ITransactionReleaseLogRepository
    {
        public TransactionReleaseLogRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TransactionReleaseLog Get(long id)
        {
            return Get(new TransactionReleaseLog { Id = id });
        }

        public TransactionReleaseLog GetByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionReleaseLog.TransactionId):C} = @TransactionId")
               .WithParameters(new { TransactionId = transactionId })
           ).FirstOrDefault();
        }
    }
}