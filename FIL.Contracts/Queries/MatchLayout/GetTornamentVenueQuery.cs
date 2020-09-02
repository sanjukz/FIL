using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MatchLayout;

namespace FIL.Contracts.Queries.MatchLayout
{
    public class GetTornamentVenueQuery : IQuery<GetTournamentVenueQueryResult>
    {
        public int eventid { get; set; }
    }
}