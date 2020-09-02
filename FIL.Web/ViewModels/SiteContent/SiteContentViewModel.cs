using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.SiteContent
{
    public class SiteContentViewModel
    {
        public FIL.Contracts.Models.FeelSiteContent Content { get; set; }
        public List<SiteBannerDetail> SiteBanners { get; set; }
        public List<City> DefaultSearchCities { get; set; }
        public List<State> DefaultSearchStates { get; set; }
        public List<FIL.Contracts.Models.Country> DefaultSearchCountries { get; set; }
    }
}
