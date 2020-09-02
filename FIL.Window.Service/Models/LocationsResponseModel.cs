using System;
using System.Collections.Generic;
using System.Text;

namespace Kz.Window.Service.Models
{
    public class LocationsResponseModel
    {
        public LocationResponse LocationResponse { get; set; }
    }
    public class LocationResponse
    {
        public long Id { get; set; }
        public List<Kz.Contracts.DataModels.CitySightSeeingLocation> Locations { get; set; }
    }
}
