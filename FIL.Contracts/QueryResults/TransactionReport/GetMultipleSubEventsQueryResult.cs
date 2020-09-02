using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TransactionReport
{
    public class GetMultipleSubEventsQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.EventDetail> SubEvents { get; set; }
        public List<FIL.Contracts.DataModels.CurrencyType> CurrencyTypes { get; set; }
    }
}