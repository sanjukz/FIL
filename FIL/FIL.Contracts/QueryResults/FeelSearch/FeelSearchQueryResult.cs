using FIL.Contracts.QueryResults.Category;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.FeelSearch
{
    public class FeelSearchQueryResult
    {
        public FeelCategoryEventQueryResult FeelAdvanceSearchQueryResult { get; set; }
        public List<FIL.Contracts.Models.Search> Countries { get; set; }
        public List<FIL.Contracts.Models.Search> States { get; set; }
        public List<FIL.Contracts.Models.Search> Cities { get; set; }
    }
}