using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class BOClosingReportQueryResult
    {
        public List<BOClosingDetail> ClosingReportDetails { get; set; }
        public List<FIL.Contracts.Models.User> Users { get; set; }
    }
}