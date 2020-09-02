using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TMS;

namespace FIL.Contracts.Queries.TMS
{
    public class SponsorOrderQuery : IQuery<SponsorOrderQueryResult>
    {
        public long EventTicketAttributeId { get; set; }
    }
}