using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using FIL.Contracts.Attributes;
using FIL.Contracts.Enums;
using FIL.Utilities.Extensions;
using FIL.Web.Core.Providers;
using Microsoft.AspNetCore.Http;

namespace FIL.Web.Feel.Providers
{
    public class SiteIdProvider : BaseSiteIdProvider
    {
        public SiteIdProvider(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

        public override Contracts.Enums.Site GetSiteId()
        {
            var hostUrl = GetHost();
            var siteId = Contracts.Enums.Site.feelaplaceSite;
            if (hostUrl.Contains("feelindia.com"))
            {
                siteId = Contracts.Enums.Site.feelIndiaSite;
            }
            else if (hostUrl.Contains("feelrajasthan.com"))
            {
                siteId = Contracts.Enums.Site.feelRajasthanSite;
            }
            else if (hostUrl.Contains("feelmaharashtra.com"))
            {
                siteId = Contracts.Enums.Site.feelMaharashtraSite;
            }
            else if (hostUrl.Contains("feeldubai.com"))
            {
                siteId = Contracts.Enums.Site.feelDubaiSite;
            }
            else if (hostUrl.Contains("feelpondicherry.com"))
            {
                siteId = Contracts.Enums.Site.feelPondicherrySite;
            }
            else if (hostUrl.Contains("feeluttarpradesh.com"))
            {
                siteId = Contracts.Enums.Site.feelUttarPradeshSite;
            }
            else if (hostUrl.Contains("feelmadhyapradesh.com"))
            {
                siteId = Contracts.Enums.Site.feelMadhyaPradeshSite;
            }
            else if (hostUrl.Contains("feelnewyork.com"))
            {
                siteId = Contracts.Enums.Site.feelNewYork;
            }
            else if (hostUrl.Contains("feellondon.com"))
            {
                siteId = Contracts.Enums.Site.feelLondon;
            }
            else if (hostUrl.Contains("feelantiguaandbarbuda.com"))
            {
                siteId = Contracts.Enums.Site.feelAntiguaandBarbuda;
            }
            else if (hostUrl.Contains("feelantigua.com"))
            {
                siteId = Contracts.Enums.Site.feelantigua;
            }
            else if (hostUrl.Contains("feelthecaribbean.com"))
            {
                siteId = Contracts.Enums.Site.feelthecaribbean;
            }
            else if (hostUrl.Contains("feelsaintlucia.com"))
            {
                siteId = Contracts.Enums.Site.feelsaintlucia;
            }
            else if (hostUrl.Contains("feelstlucia.com"))
            {
                siteId = Contracts.Enums.Site.feelstlucia;
            }
            else if (hostUrl.Contains("dev.feelitlive") || hostUrl.Contains("demo"))
            {
                siteId = Contracts.Enums.Site.FeelDevelopmentSite;
            }
            return siteId;
        }
    }
}
