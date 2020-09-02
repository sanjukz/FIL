using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CustomerUpdate;

namespace FIL.Contracts.Queries.CustomerUpdate
{
    public class CustomerUpdateQuery : IQuery<CustomerUpdateQueryResult>
    {
        public Site SiteId { get; set; }
    }
}