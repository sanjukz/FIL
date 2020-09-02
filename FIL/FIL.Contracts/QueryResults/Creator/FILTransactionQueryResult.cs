using System.Collections.Generic;

namespace FIL.Contracts.QueryResults
{
    public class FILTransactionQueryResult
    {
        public FIL.Contracts.Models.Creator.FILTransactionLocator FILTransactionLocator { get; set; }
        public bool Success { get; set; }
    }
}