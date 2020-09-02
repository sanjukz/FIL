using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using FIL.Contracts.Attributes;
using FIL.Contracts.Enums;
using FIL.Web.Core.Providers;
using Microsoft.AspNetCore.Http;

namespace FIL.Web.Kitms.Feel.Providers
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
            if (hostUrl.Contains("devadmin.feelitlive"))
            {
                siteId = Contracts.Enums.Site.FeelDevelopmentSite;
            }
            return siteId;
        }
    }
}
