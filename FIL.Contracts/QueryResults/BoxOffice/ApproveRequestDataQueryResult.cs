using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class ApproveRequestDataQueryResult
    {
        public IEnumerable<VoidRequestDetail> VoidRequestDetail { get; set; }
        public IEnumerable<FIL.Contracts.Models.Transaction> Transaction { get; set; }
        public List<ApproveVoidRequestContainer> ApproveVoidRequestContainer { get; set; }
    }
}