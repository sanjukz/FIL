using FIL.Contracts.Queries.SiteInfo;
using FIL.Contracts.QueryResults.SiteInfo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.SiteInfo
{
    public class SiteQueryHandler : IQueryHandler<SiteQuery, SiteQueryResult>
    {
        public SiteQueryHandler()
        {
        }

        public SiteQueryResult Handle(SiteQuery query)
        {
            var siteResult = Enum.GetValues(typeof(FIL.Contracts.Enums.Site)).Cast<FIL.Contracts.Enums.Site>();
            List<FIL.Contracts.Models.SiteDetail> sitedataList = new List<FIL.Contracts.Models.SiteDetail>();
            foreach (var item in siteResult)
            {
                FIL.Contracts.Models.SiteDetail site = new Contracts.Models.SiteDetail();
                site.Id = (int)item;
                site.SiteName = item.ToString();
                sitedataList.Add(site);
            }
            return new SiteQueryResult() { SiteData = sitedataList };
        }
    }
}