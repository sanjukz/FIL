using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TicketLookup;

namespace FIL.Contracts.Queries.TicketLookup
{
    public class TicketLookupEmailDetailQuery : IQuery<TicketLookupEmailDetailQueryResult>
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}