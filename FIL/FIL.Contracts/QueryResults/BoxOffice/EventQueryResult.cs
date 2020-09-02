using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class EventQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.Event> Events { get; set; }
    }
}