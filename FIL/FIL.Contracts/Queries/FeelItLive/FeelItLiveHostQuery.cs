using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;

namespace FIL.Contracts.Queries
{
    public class FeelItLiveHostQuery : IQuery<FeelItLiveHostQueryResult>
    {
        public string Email { get; set; }
    }
}