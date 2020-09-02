using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Category
{
    public class FeelCategoryEventQueryResult
    {
        public List<CategoryEventContainer> CategoryEvents { get; set; }
        public CountryDescription CountryDescription { get; set; }
        public FIL.Contracts.DataModels.CityDescription CityDescription { get; set; }
        public List<CountryContentMapping> CountryContentMapping { get; set; }
    }
}