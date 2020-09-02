using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TicketCategoryTypes
{
    public class TicketCategoryTypesQueryResult
    {
        public List<FIL.Contracts.DataModels.TicketCategoryType> TicketCategoryTypes { get; set; }
        public List<FIL.Contracts.DataModels.TicketCategorySubType> TicketCategorySubTypes { get; set; }
    }
}