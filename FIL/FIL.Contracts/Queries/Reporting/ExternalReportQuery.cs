using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Reporting;
using System;

namespace FIL.Contracts.Queries.Reporting
{
    public class ExternalReportQuery : IQuery<ExternalReportQueryResult>
    {
        public Guid UserAltId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public ReportExportStatus? ExportStatus { get; set; }
    }
}