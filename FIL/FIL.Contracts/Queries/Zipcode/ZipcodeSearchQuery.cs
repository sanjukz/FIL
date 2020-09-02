using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;

namespace FIL.Contracts.Queries.Zipcode
{
    public class ZipcodeSearchQuery : IQuery<ZipcodeSearchQueryResult>
    {
        public string Zipcode { get; set; }
    }
}