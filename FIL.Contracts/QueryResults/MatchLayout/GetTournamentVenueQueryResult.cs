using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.MatchLayout
{
    public class GetTournamentVenueQueryResult
    {
        public List<FIL.Contracts.Models.Venue> venues { get; set; }
        public List<FIL.Contracts.DataModels.EventFeeTypeMapping> feeTypeMapping { get; set; }
        public List<string> channels { get; set; }
        public List<string> feeType { get; set; }
        public List<string> valueType { get; set; }
    }
}