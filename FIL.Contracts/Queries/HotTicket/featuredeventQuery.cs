using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;

namespace FIL.Contracts.Queries.HotTicket
{
    public class FeaturedEventQuery : IQuery<FeaturedEventQueryResults>
    {
        public Site SiteId { get; set; }
    }
}