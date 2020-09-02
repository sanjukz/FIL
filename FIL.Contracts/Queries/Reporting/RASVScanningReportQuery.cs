using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Reporting;
using System;

namespace FIL.Contracts.Queries.Reporting
{
    public class RASVScanningReportQuery : IQuery<RASVScanningReportQueryResult>
    {
        public Guid UserAltId { get; set; }
        public Guid EventAltId { get; set; }
        public long EventDetailId { get; set; }
        public Guid EventDetailAltId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int PageNumber { get; set; }
        public int NoRecordsPerPage { get; set; }
        public bool SearchBarcode { get; set; }
        public string Barcode { get; set; }
    }
}