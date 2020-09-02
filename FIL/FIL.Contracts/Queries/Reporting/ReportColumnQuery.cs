using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Reporting;
using System;

namespace FIL.Contracts.Queries.Reporting
{
    public class ReportColumnQuery : IQuery<ReportColumnQueryResult>
    {
        public Guid UserAltId { get; set; }
        public Guid EventAltId { get; set; }
        public int ReportId { get; set; }
    }
}