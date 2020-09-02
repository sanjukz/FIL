using FIL.Contracts.Models.TournamentLayouts;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TournamentLayout
{
    public class TournamentLayoutViewSeatLayoutQueryResult
    {
        public List<TournamentLayoutSectionSeatModel> MasterVenueRows { get; set; }
        public bool IsSeatLayout { get; set; }
    }
}