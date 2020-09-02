using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Recommendation
{
    public class RecommendationQueryResult
    {
        public List<CategoryEventContainer> CategoryEvents { get; set; }
    }
}