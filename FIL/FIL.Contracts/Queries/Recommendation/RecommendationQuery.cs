using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Recommendation;
using System;

namespace FIL.Contracts.Queries.Recommendation
{
    public class RecommendationQuery : IQuery<RecommendationQueryResult>
    {
        public string Id { get; set; }
        public Int32 EventCategoryId { get; set; }
        public Site SiteId { get; set; }
    }
}