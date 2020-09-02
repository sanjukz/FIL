using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TournamentLayout;

namespace FIL.Contracts.Queries.TournamentLayout
{
    public class TournamentLayoutSaveSeatQuery : IQuery<TournamentLayoutSaveSeatQueryResult>
    {
        public string xmlData { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public bool ShouldSeatInsert { get; set; }
    }
}