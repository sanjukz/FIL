using FIL.Contracts.Models.MatchLevel;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.MatchLayout
{
    public class MatchLevelViewSeatQueryResult
    {
        public List<MatchLayoutSeatModel> MatchLevelRows { get; set; }
        public bool IsSeatLayout { get; set; }
    }
}