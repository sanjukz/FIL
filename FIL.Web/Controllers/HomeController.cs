using FIL.Configuration;
using FIL.Logging;
using FIL.Web.Core.Controllers;
using FIL.Web.Core.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Feel.Controllers
{
    public class HomeController : BaseHomeController
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger _logger;
        private readonly Modules.SiteExtensions.IGeoRedirection _geoRedirection;
        public HomeController(ISiteIdProvider siteIdProvider, ISettings settings, IHttpContextAccessor context, ILogger logger, Modules.SiteExtensions.IGeoRedirection geoRedirection, IDynamicSourceProvider dynamicSourceProvider)
            : base(siteIdProvider, settings, dynamicSourceProvider)
        {
            _context = context;
            _logger = logger;
            _geoRedirection = geoRedirection;
        }

        public override IActionResult Index(Contracts.Enums.Site? siteId)
        {
            siteId = siteId ?? _siteIdProvider.GetSiteId();
            if (Contracts.Enums.Site.DevelopmentSite == siteId)
            {
                siteId = Contracts.Enums.Site.feelaplaceSite;
                var qs = _context.HttpContext.Request.Query;
                if (!qs.ContainsKey("verificationToken") || qs["verificationToken"].ToString().ToUpper() != "429BBA30-8E23-4222-B424-559F64C34D7B")
                {
                    return Unauthorized();
                }
            }

            //making sure function is called
            //GetIPAddress method obtains ip address            
            string ip = _geoRedirection.GetRequestIP();
            if (ip != null && ip != "::1" && ip != "127.0.0.1")
            {
                _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - RedirectOnIP() called.");
                string urlTo = GetRedirectionUrl(ip);
                //checking URL returned from function
                _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - Redirection value returned from function RedirectOnIP() is " + urlTo);
                if (urlTo != string.Empty)
                {
                    return Redirect(urlTo);
                }
            }
            return base.Index(siteId);
        }

        private string GetRedirectionUrl(string ip)
        {
            string returnUrl = string.Empty;
            try
            {
                if (_context.HttpContext.Request.Cookies["geo_url"] == null)
                {
                    returnUrl = _geoRedirection.RedirectOnIP(ip);

                    //create cookie so that frontend can show selected value.
                    _context.HttpContext.Response.Cookies.Append("geo_url", returnUrl,
                   new Microsoft.AspNetCore.Http.CookieOptions
                   {
                       Expires = System.DateTimeOffset.Now.AddDays(1)
                   });
                    //store user IP.
                    _context.HttpContext.Response.Cookies.Append("geo_user_ip", ip,
                   new Microsoft.AspNetCore.Http.CookieOptions
                   {
                       Expires = System.DateTimeOffset.Now.AddDays(1)
                   });
                }
                else
                {
                    // returnUrl = _context.HttpContext.Request.Cookies["geo_url"];
                    returnUrl = _geoRedirection.RedirectOnIP(ip);
                    //extend cookies if IP was same else remove
                    if (ip == _context.HttpContext.Request.Cookies["geo_user_ip"])
                    {
                        //extend geo_url cookie expiry by a day - remove and recreate
                        //remove cookies
                        _context.HttpContext.Response.Cookies.Delete("geo_url");
                        _context.HttpContext.Response.Cookies.Delete("geo_user_ip");
                        //create cookie so that frontend can show selected value.
                        _context.HttpContext.Response.Cookies.Append("geo_url", returnUrl,
                       new Microsoft.AspNetCore.Http.CookieOptions
                       {
                           Expires = System.DateTimeOffset.Now.AddDays(1)
                       });
                        //store user IP.
                        _context.HttpContext.Response.Cookies.Append("geo_user_ip", ip,
                       new Microsoft.AspNetCore.Http.CookieOptions
                       {
                           Expires = System.DateTimeOffset.Now.AddDays(1)
                       });
                    }
                }
                //check if it is dev env
                if (returnUrl.Contains("dev."))
                {
                    returnUrl = returnUrl.Replace("www.", "");
                }
            }
            catch (System.Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);

                if (Contracts.Enums.Site.FeelDevelopmentSite == _siteIdProvider.GetSiteId())
                {
                    returnUrl = "https://dev.feelitlive.com";
                }
                else
                {
                    returnUrl = "https://feelitlive.com";
                }
            }
            return returnUrl;
        }

    }
}
