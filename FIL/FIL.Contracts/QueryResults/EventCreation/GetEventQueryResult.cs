using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.EventCreation
{
    public class GetEventQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.Event> Event { get; set; }
    }
}