using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;

namespace FIL.Contracts.Queries.State
{
    public class StateSearchQuery : IQuery<StateSearchQueryResult>
    {
        public string Name { get; set; }
    }
}