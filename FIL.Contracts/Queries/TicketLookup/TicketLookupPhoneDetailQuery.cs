using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TicketLookup;

namespace FIL.Contracts.Queries.TicketLookup
{
    public class TicketLookupPhoneDetailQuery : IQuery<TicketLookupPhoneDetailQueryResult>
    {
        public string PhoneNumber { get; set; }
    }
}