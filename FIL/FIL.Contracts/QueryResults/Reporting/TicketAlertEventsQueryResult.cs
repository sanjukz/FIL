using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Reporting
{
    public class TicketAlertEventsQueryResult
    {
        public IEnumerable<Event> Events { get; set; }
    }
}