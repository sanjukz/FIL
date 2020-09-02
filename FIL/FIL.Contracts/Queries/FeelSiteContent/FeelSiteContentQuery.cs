using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.FeelSiteContent;

namespace FIL.Contracts.Queries.FeelSiteContent
{
    public class FeelSiteContentQuery : IQuery<FeelSiteContentQueryResult>
    {
        public Site SiteId { get; set; }
    }
}