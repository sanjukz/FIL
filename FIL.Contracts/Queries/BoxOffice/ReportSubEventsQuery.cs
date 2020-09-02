using FIL.Contracts.Interfaces.Queries;
using System;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class ReportSubEventsQuery : IQuery<ReportSubEventsQueryResult>
    {
        public Guid UserAltId { get; set; }
        public Guid EventAltId { get; set; }
    }
}