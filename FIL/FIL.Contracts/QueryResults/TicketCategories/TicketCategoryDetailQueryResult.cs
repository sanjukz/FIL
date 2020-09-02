using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TicketCategories
{
    public class TicketCategoryDetailQueryResult
    {
        public List<FIL.Contracts.Models.TicketCategoryDetail> TicketCategoryDetails { get; set; }
        public List<FIL.Contracts.Models.TicketCategory> TicketCategories { get; set; }
    }
}