using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MatchLayout;

namespace FIL.Contracts.Queries.MatchLayout
{
    public class MatchLevelViewSeatQuery : IQuery<MatchLevelViewSeatQueryResult>
    {
        public int Id { get; set; }
    }
}