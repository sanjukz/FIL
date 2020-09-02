using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Commands.Reporting
{
    public class ExternalReportCommand : BaseCommand
    {
        public Guid UserAltId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public ReportExportStatus? CurrentExportStatus { get; set; }
        public ReportExportStatus? NewExportStatus { get; set; }
    }
}