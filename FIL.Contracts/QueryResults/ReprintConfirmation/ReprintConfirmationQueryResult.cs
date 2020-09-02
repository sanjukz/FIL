using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.ReprintConfirmation
{
    public class ReprintConfirmationQueryResult
    {
        public List<FIL.Contracts.Models.Transaction> Transaction { get; set; }
        public bool Success { get; set; }
    }
}