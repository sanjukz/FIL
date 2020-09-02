using FIL.Contracts.Models;
using FIL.Contracts.QueryResults;
using System.Collections.Generic;

namespace FIL.Web.Feel.ViewModels
{
    public class FeelNearbyViewModel
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public List<VenueWithCityName> NearbyPlaces { get; set; }
    }
}