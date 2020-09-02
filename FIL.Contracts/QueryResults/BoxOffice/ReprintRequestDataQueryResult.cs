using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class ReprintRequestDataQueryResult
    {
        public FIL.Contracts.Models.User User { get; set; }
        public IEnumerable<ReprintRequestDetail> ReprintRequestDetail { get; set; }
        public IEnumerable<ReprintRequest> ReprintRequest { get; set; }
    }
}