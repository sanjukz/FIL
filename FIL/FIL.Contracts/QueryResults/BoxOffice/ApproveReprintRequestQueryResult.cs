using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class ApproveReprintRequestQueryResult
    {
        public List<ReprintRequest> ReprintRequests { get; set; }
        public List<ReprintRequestDetail> ReprintRequestDetail { get; set; }
        public List<FIL.Contracts.Models.User> Users { get; set; }
    }
}