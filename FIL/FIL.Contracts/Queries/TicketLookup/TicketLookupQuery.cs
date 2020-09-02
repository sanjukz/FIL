using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TicketLookup;

namespace FIL.Contracts.Queries.TicketLookup
{
    public class TicketLookupQuery : IQuery<TicketLookupQueryResult>
    {
        public long TransactionId { get; set; }
    }
}