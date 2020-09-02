using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CustomerIpDetails;

namespace FIL.Contracts.Queries.CustomerIpDetails
{
    public class CustomerIpDetailQuery : IQuery<CustomerIpDetailQueryResult>
    {
        public string Ip { get; set; }
    }
}