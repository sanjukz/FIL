using System.Collections.Generic;

namespace FIL.Web.Feel.ViewModels.MasterBudgetRange
{
    public class MasterBudgetRangeResponseViewModel
    {
        public List<FIL.Contracts.DataModels.MasterBudgetRange> MasterBudgetRanges { get; set; }
        public List<FIL.Contracts.DataModels.CurrencyType> CurrencyTypes { get; set; }
    }
}