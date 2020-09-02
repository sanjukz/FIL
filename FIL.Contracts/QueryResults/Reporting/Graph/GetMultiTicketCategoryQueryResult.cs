using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Reporting.Graph
{
    public class GetMultiTicketCategoryQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.TicketCategory> TicketCategories { get; set; }
        public IEnumerable<FIL.Contracts.Models.CurrencyType> CurrencyTypes { get; set; }
    }
}