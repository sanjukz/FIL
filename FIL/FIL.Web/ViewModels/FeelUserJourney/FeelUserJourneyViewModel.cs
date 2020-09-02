using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.FeelUserJourney
{
    public class FeelUserJourneyViewModel
    {
        public bool Success { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public GeoData GeoData { get; set; }
        public List<DynamicPlaceSections> DynamicPlaceSections { get; set; }
        public DynamicPlaceSections AllPlaceTiles { get; set; }
        public CountryPageDetail ContryPageDetails { get; set; }        
        public string SearchValue { get; set; }
    }
}
