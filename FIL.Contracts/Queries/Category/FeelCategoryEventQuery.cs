using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.Models;
using FIL.Contracts.QueryResults.Category;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.Category
{
    public class FeelCategoryEventQuery : IQuery<FeelCategoryEventQueryResult>
    {
        public int EventCategoryId { get; set; }
        public Site SiteId { get; set; }
        public int PageNumber { get; set; }
        public bool IsAll { get; set; }
        public bool IsAllOnline { get; set; }
        public bool IsSearch { get; set; }
        public string Search { get; set; }
        public string slug { get; set; }
        public bool IsSimilarListing { get; set; }
        public bool IsCountryLandingPage { get; set; }
        public bool IsCityLandingPage { get; set; }
        public string CountryName { get; set; }
        public List<EventCategory> EventCategories { get; set; }
    }
}