using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Reporting;
using System;

namespace FIL.Contracts.Queries.Reporting
{
    public class ReportEventsQuery : IQuery<ReportEventsQueryResult>
    {
        public Guid AltId { get; set; }
        public bool IsFeel { get; set; }
    }
}