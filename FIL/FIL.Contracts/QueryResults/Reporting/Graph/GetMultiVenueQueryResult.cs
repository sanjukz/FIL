using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Reporting.Graph
{
    public class GetMultiVenueQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.Venue> Venues { get; set; }
    }
}