using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Reporting;
using System;

namespace FIL.Contracts.Queries.Reporting
{
    public class TicketAlertEventsQuery : IQuery<TicketAlertEventsQueryResult>
    {
        public Guid AltId { get; set; }
    }
}