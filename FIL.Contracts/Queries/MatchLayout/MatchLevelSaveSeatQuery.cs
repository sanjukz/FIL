using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MatchLayout;

namespace FIL.Contracts.Queries.MatchLayout
{
    public class MatchLevelSaveSeatQuery : IQuery<MatchLevelSaveSeatQueryResult>
    {
        public string xmlData { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public bool ShouldSeatInsert { get; set; }
    }
}