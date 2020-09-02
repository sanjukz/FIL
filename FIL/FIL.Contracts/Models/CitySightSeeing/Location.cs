using System.Collections.Generic;

namespace FIL.Contracts.Models.CitySightSeeing
{
    public class Location
    {
        public string location_name { get; set; }
        public string country_name { get; set; }
    }

    public class LocationData
    {
        public List<Location> locations { get; set; }
    }

    public class LocationList
    {
        public string response_type { get; set; }
        public LocationData data { get; set; }
    }
}