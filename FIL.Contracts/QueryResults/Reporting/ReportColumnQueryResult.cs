using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Reporting
{
    public class ReportColumnQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.ReportingColumn> ReportColumns { get; set; }
    }
}