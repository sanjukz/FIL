using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.CategoryEventSearch;

namespace FIL.Contracts.Queries.CategoryEventSearch
{
    public class CategoryEventSearchQuery : IQuery<CategoryEventSearchQueryResult>
    {
        public string Name { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public Site SiteId { get; set; }
    }
}