using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ITransactionBarcodeReversalLogRepository : IOrmRepository<TransactionBarcodeReversalLog, TransactionBarcodeReversalLog>
    {
        TransactionBarcodeReversalLog Get(long id);
    }

    public class TransactionBarcodeReversalLogRepository : BaseLongOrmRepository<TransactionBarcodeReversalLog>, ITransactionBarcodeReversalLogRepository
    {
        public TransactionBarcodeReversalLogRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TransactionBarcodeReversalLog Get(long id)
        {
            return Get(new TransactionBarcodeReversalLog { Id = id });
        }

        public IEnumerable<TransactionBarcodeReversalLog> GetAll()
        {
            return GetAll(null);
        }
    }
}