using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.MatchLayout
{
    public class MatchLayoutSectionDetailsQueryResult
    {
        public List<SectionDetailsByTournamentLayout> sectionDetailsByTournamentLayout { get; set; }
    }
}