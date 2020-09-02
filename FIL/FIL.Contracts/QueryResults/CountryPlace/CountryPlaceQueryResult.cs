using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.CountryPlace
{
    public class CountryPlaceQueryResult
    {
        public List<FIL.Contracts.DataModels.CountryPlace> CountryPlaces { get; set; }
        public List<FIL.Contracts.DataModels.CountryPlace> CountryCategoryCounts { get; set; }
    }
}