using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResult.MatchLayout;

namespace FIL.Contracts.Queries.MatchLayout
{
    public class MatchLayoutGetMatchesQuery : IQuery<MatchLayoutGetMatchesQueryResult>
    {
        public int EventId { get; set; }
        public int VenueId { get; set; }
    }
}