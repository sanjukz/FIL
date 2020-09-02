using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Reporting
{
    public class ReportEventsQueryResult
    {
        public IEnumerable<Event> Events { get; set; }
    }
}