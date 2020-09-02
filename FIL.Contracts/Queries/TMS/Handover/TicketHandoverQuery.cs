using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TMS.Handover;

namespace FIL.Contracts.Queries.TMS.Handover
{
    public class TicketHandoverQuery : IQuery<TicketHandoverQueryResult>
    {
        public long TransactionId { get; set; }
    }
}