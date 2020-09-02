using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.MasterBudgetRange
{
    public class MasterBudgetRangeQueryResult
    {
        public List<FIL.Contracts.DataModels.MasterBudgetRange> MasterBudgetRanges { get; set; }
        public List<FIL.Contracts.DataModels.CurrencyType> CurrencyTypes { get; set; }
    }
}