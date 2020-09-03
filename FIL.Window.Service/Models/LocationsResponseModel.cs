using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Window.Service.Models
{
    public class LocationsResponseModel
    {
        public LocationResponse LocationResponse { get; set; }
    }
    public class LocationResponse
    {
        public long Id { get; set; }
        public List<FIL.Contracts.DataModels.CitySightSeeingLocation> Locations { get; set; }
    }
}
