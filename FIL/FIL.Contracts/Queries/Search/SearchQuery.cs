using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Search;

namespace FIL.Contracts.Queries.Search
{
    public class SearchQuery : IQuery<SearchQueryResult>
    {
        public string Name { get; set; }
    }
}