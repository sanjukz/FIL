using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CustomerUpdate;

namespace FIL.Contracts.Queries.CustomerUpdate
{
    public class SitePropertyQuery : IQuery<SitePropertyQueryResult>
    {
        public Site SiteId { get; set; }
    }
}