using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.DynamicLayout
{
    public class DynamicLayoutQueryResult
    {
        public List<DynamicStadiumCoordinate> DynamicStadiumCoordinate { get; set; }
        public List<DynamicStadiumTicketCategoriesDetails> DynamicStadiumTicketCategoriesDetails { get; set; }
        public List<FIL.Contracts.DataModels.PathType> PathTypes { get; set; }
    }
}