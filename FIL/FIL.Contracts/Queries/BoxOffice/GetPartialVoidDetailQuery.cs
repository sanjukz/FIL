using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class GetPartialVoidDetailQuery : IQuery<ReprintRequestQueryResult>
    {
        public long TransactionId { get; set; }
        public string BarcodeNumber { get; set; }
    }
}