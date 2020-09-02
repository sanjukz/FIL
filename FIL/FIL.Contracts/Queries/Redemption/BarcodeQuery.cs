using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Redemption;

namespace FIL.Contracts.Queries.Redemption
{
    public class BarcodeQuery : IQuery<BarcodeQueryResult>
    {
        public string BarcodeNumber { get; set; }
    }
}