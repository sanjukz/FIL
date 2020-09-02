using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Itinerary
{
    public class ItineraryQueryResult
    {
        public List<FIL.Contracts.DataModels.Itinerary> ItinerarySearchData { get; set; }
        public List<FIL.Contracts.DataModels.FeelState> FeelStateData { get; set; }
    }
}