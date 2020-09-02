using System;
using System.Collections.Generic;
using System.Text;
using FIL.Web.Core.UrlsProvider;
using Microsoft.AspNetCore.Http;
using SimpleMvcSitemap.Routing;

namespace FIL.Web.Core.Providers.Sitemap
{
    public class BaseUrlProvider : IBaseUrlProvider
    {
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly ISiteUrlsProvider _siteUrlsProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseUrlProvider(ISiteIdProvider siteIdProvider, ISiteUrlsProvider siteUrlsProvider, IHttpContextAccessor httpContextAccessor)
        {
            _siteIdProvider = siteIdProvider;
            _siteUrlsProvider = siteUrlsProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public Uri BaseUrl => new Uri(_httpContextAccessor.HttpContext.Request.Host.Value.Contains("localhost") == true ? "http://" + _httpContextAccessor.HttpContext.Request.Host.Value : "https://" + _httpContextAccessor.HttpContext.Request.Host.Value);
    }
}
