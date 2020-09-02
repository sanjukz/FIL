using FIL.Contracts.Models.Report;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Reporting
{
    public class ExternalReportQueryResult
    {
        public List<ExternalTranscationReportContainer> externalTranscationReportContainer { get; set; }
        public bool IsValid { get; set; }
    }
}