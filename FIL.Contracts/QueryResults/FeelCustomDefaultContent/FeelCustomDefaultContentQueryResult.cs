using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.FeelCustomDefaultContent
{
    public class FeelCustomDefaultContentQueryResult
    {
        public FIL.Contracts.Models.FeelSiteContent FeelSiteContent { get; set; }
        public List<SiteBannerDetail> SiteBanners { get; set; }
        public List<FIL.Contracts.Models.Country> Countries { get; set; }
        public List<State> States { get; set; }
        public List<City> Cities { get; set; }
    }
}