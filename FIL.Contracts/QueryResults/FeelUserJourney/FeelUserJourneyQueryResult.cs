using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.FeelUserJourney
{
    public class FeelUserJourneyQueryResult
    {
        public List<SubCategory> SubCategories { get; set; }
        public GeoData GeoData { get; set; }
        public List<DynamicPlaceSections> DynamicPlaceSections { get; set; }
        public DynamicPlaceSections AllPlaceTiles { get; set; }
        public CountryPageDetail ContryPageDetails { get; set; }
        public bool Success { get; set; }
        public string SearchValue { get; set; }
    }
}