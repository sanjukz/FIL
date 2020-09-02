using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TMS.Handover;

namespace FIL.Contracts.Queries.TMS.Handover
{
    public class SponsorTicketQuery : IQuery<SponsorTicketQueryResult>
    {
        public long TransactionId { get; set; }
    }
}