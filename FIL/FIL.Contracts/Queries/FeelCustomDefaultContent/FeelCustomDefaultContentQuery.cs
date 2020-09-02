using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.FeelCustomDefaultContent;

namespace FIL.Contracts.Queries.FeelCustomDefaultContent
{
    public class FeelCustomDefaultContentQuery : IQuery<FeelCustomDefaultContentQueryResult>
    {
        public Site SiteId { get; set; }
    }
}