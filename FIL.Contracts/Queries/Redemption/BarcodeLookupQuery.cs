using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Redemption;

namespace FIL.Contracts.Queries.Redemption
{
    public class BarcodeLookupQuery : IQuery<BarcodeLookupQueryResult>
    {
        public long TransactionId { get; set; }
    }
}