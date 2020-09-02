using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Algolia
{
    public class GetAllPlacesQueryResult
    {
        public List<PlaceDetail> AllPlaces { get; set; }
        public List<FIL.Contracts.DataModels.Itinerary> GetAllCities { get; set; }
    }
}