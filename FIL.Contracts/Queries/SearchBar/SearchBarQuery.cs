using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.SearchBar;

namespace FIL.Contracts.Queries.SearchBar
{
    public class SearchBarQuery : IQuery<SearchBarQueryResults>
    {
        public string Search { get; set; }
    }
}