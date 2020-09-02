using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.SiteInfo;

namespace FIL.Contracts.Queries.SiteInfo
{
    public class SiteQuery : IQuery<SiteQueryResult>
    {
        public string Name { get; set; }
    }
}