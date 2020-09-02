using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults
{
    public class FeelNearbyQueryResult
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public List<VenueWithCityName> NearbyPlaces { get; set; }
    }

    public class VenueWithCityName : Venue
    {
        public string City { get; set; }
    }
}