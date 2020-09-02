using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.ASI;

namespace FIL.Contracts.Queries.ASI
{
    public class ASITicketInfoQuery : IQuery<ASITicketInfoQueryResult>
    {
        public long TransactionId { get; set; }
        public string TransactionIds { get; set; }
        public bool IsASIQR { get; set; }
    }
}