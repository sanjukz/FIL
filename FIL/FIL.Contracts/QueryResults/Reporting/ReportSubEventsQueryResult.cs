using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.Reporting
{
    public class ReportSubEventsQueryResult
    {
        public IEnumerable<EventDetail> SubEvents { get; set; }
    }
}