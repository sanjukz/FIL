using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TMS;

namespace FIL.Contracts.Queries.TMS
{
    public class BarcodeDetailQuery : IQuery<BarcodeDetailQueryResult>
    {
        public long TransactionId { get; set; }
    }
}