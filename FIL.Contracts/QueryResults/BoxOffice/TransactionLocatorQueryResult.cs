using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class TransactionLocatorQueryResult
    {
        public IEnumerable<TransactionInfo> TransactionInfos { get; set; }
    }
}