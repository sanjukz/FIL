using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.DyanamicContent;

namespace FIL.Contracts.Queries
{
    public class DynamicContentQuery : IQuery<DynamicContentQueryResult>
    {
        public string Url { get; set; }
        public string QueryString { get; set; }
    }
}