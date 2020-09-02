using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Reporting.Graph
{
    public class GetMultiSubeventQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.EventDetail> EventDetails { get; set; }
    }
}