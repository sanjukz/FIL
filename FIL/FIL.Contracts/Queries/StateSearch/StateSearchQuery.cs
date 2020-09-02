using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.StateSearch;

namespace FIL.Contracts.Queries.StateSearch
{
    public class StateSearchQuery : IQuery<StateSearchQueryResult>
    {
        public string Name { get; set; }
    }
}