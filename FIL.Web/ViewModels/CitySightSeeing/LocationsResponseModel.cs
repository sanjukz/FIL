using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.CitySightSeeing
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
