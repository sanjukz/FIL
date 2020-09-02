using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class ApprovePartialVoidRequestDataQueryResult
    {
        public List<PartialVoidRequestDetail> PartialVoidRequestDetail { get; set; }
        public List<FIL.Contracts.Models.User> Users { get; set; }
    }
}