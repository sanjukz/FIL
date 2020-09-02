using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TournamentLayout
{
    public class TournamentSectionDetailsQueryResult
    {
        public List<TournamentSectionDetailsByVenueLayout> SectionDetailsByVenueLayout { get; set; }
        public bool IsExistTournament { get; set; }
    }
}