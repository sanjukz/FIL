using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.SiteBanner;

namespace FIL.Contracts.Queries.SiteBanner
{
    public class SiteBannerQuery : IQuery<SiteBannerQueryResult>
    {
        public Site SiteId { get; set; }
    }
}