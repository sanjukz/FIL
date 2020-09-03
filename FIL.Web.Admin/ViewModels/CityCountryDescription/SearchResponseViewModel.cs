using FIL.Web.Admin.ViewModels.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.CityCountryDescription
{
    public class SearchResponseViewModel
    {
        public List<FIL.Contracts.DataModels.Itinerary> ItinerarySerchData { get; set; }
        public List<FIL.Contracts.DataModels.FeelState> FeelStateData { get; set; }
    }
}
