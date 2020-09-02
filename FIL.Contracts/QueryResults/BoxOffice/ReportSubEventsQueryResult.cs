using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class ReportSubEventsQueryResult
    {
        public IEnumerable<EventDetail> SubEvents { get; set; }
    }
}