using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;

namespace FIL.Contracts.Queries.Stand
{
    public class StandSearchQuery : IQuery<StandSearchQueryResult>
    {
        public string SectionName { get; set; }
    }
}