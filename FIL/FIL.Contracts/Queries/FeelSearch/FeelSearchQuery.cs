using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.FeelSearch;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.FeelSearch
{
    public class FeelSearchQuery : IQuery<FeelSearchQueryResult>
    {
        public string Name { get; set; }
        public bool IsAdvanceSearch { get; set; }
        public List<Guid> PlaceAltIds { get; set; }
        public Site SiteId { get; set; }
        public SiteLevel SiteLevel { get; set; } = SiteLevel.Global;
    }
}