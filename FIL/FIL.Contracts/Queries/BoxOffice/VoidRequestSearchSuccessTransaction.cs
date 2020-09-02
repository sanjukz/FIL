using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.VoidRequestSuccessTransaction;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class VoidRequestSearchSucessTransaction : IQuery<VoidRequestSuccessTransaction>
    {
        public long TransactionId { get; set; }
    }
}