using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TournamentLayout;

namespace FIL.Contracts.Queries.TournamentLayout
{
    public class TournamentSectionDetailsQuery : IQuery<TournamentSectionDetailsQueryResult>
    {
        public int EventId { get; set; }
        public int VenueId { get; set; }
        public bool IsTournamentEdit { get; set; }
    }
}