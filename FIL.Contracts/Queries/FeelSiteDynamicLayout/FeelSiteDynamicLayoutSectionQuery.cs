using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;

namespace FIL.Contracts.Queries
{
    public class FeelSiteDynamicLayoutSectionQuery : IQuery<FeelSiteDynamicLayoutSectionQueryResult>
    {
        public int PageId { get; set; }
    }
}