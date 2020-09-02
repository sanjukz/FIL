using FIL.Configuration;
using FIL.Contracts.Extension;
using FIL.Logging;
using FIL.Web.Core.Providers;
using FIL.Web.Core.ViewModels.GeoLocation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Web.Feel.Modules.SiteExtensions
{
    public interface IGeoRedirection
    {
        string GetRequestIP(bool tryUseXForwardHeader = true);
        string RedirectOnIP(string ip);
    }
    public class GeoRedirection : IGeoRedirection
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger _logger;
        public GeoRedirection(IHttpContextAccessor context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public string RedirectOnIP(string ip)
        {
            try
            {
                //Request.Host.ToString() value
                _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - Request.Host.ToString() is " + _context.HttpContext.Request.Host.ToString());

                string _host = _context.HttpContext.Request.Host.Host;
                string _scheme = _context.HttpContext.Request.Scheme;
                string _path = _context.HttpContext.Request.Path.ToString();
                string _queryString = _context.HttpContext.Request.QueryString.ToString();

                //url used for if condition is 
                _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - url used for if condition is " + _context.HttpContext.Request.Host.ToString());

                //redirect only in dev and prod mode and not local.
                if (_host.ToLower().Contains("feelitlive.com") && _host.ToLower().EndsWith(".com"))
                {
                    _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - IP is " + ip);
                    //This is then passed on to getGeoLocation
                    var locationDetails = getGeoLocation(ip);

                    //check if its prod or dev redirect.
                    bool _isDev = true;
                    if (!_host.ToLower().Contains("dev."))
                        _isDev = false;

                    string _baseurl = string.Empty;

                    //replacement keyword to change dev to prod url --hardcoding 
                    string _devurlSuffix = "dev.";

                    if (locationDetails.country_tld.ToLower().Contains("uk"))
                    {
                        _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - uk site ");
                        _baseurl = _isDev ? EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevUkSite) : EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevUkSite).Replace(_devurlSuffix, "");
                    }
                    else if (locationDetails.country_tld.ToLower().Contains("in"))
                    {
                        _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - in site ");
                        _baseurl = _isDev ? EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevIndiaSite) : EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevIndiaSite).Replace(_devurlSuffix, "");
                    }
                    else if (locationDetails.country_tld.ToLower().Contains("au"))
                    {
                        _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - au site ");
                        _baseurl = _isDev ? EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevAustraliaSite) : EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevAustraliaSite).Replace(_devurlSuffix, "");
                    }
                    else if (locationDetails.country_tld.ToLower().Contains("de"))
                    {
                        _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - de site ");
                        _baseurl = _isDev ? EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevGermanSite) : EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevGermanSite).Replace(_devurlSuffix, "");
                    }
                    else if (locationDetails.country_tld.ToLower().Contains("es"))
                    {
                        _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - es site ");
                        _baseurl = _isDev ? EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevSpainSite) : EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevSpainSite).Replace(_devurlSuffix, "");
                    }
                    else if (locationDetails.country_tld.ToLower().Contains("fr"))
                    {
                        _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - fr site ");
                        _baseurl = _isDev ? EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevFranceSite) : EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevFranceSite).Replace(_devurlSuffix, "");
                    }
                    else if (locationDetails.country_tld.ToLower().Contains("nz"))
                    {
                        _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - nz site ");
                        _baseurl = _isDev ? EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevNewZealandSite) : EnumExtension.DescriptionAttr(Contracts.Enums.SiteUrlDev.DevNewZealandSite).Replace(_devurlSuffix, "");
                    }
                    else
                    {
                        _logger.Log(Logging.Enums.LogCategory.Debug, ip + " Web.Feel -IPRedirection - .com site ");
                        return string.Empty;
                    }

                    _baseurl = _baseurl + _path + (!string.IsNullOrEmpty(_queryString) ? _queryString : string.Empty);
                    return _baseurl;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (System.Exception ex)
            {
                //need to log exception here.
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return string.Empty;
            }
        }

        public string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = SplitCsv(GetHeaderValueAs<string>("X-Forwarded-For")).FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (IsNullOrWhitespace(ip) && _context.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _context.HttpContext.Connection.RemoteIpAddress.ToString();

            if (IsNullOrWhitespace(ip))
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (IsNullOrWhitespace(ip))
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        private T GetHeaderValueAs<T>(string headerName)
        {
            Microsoft.Extensions.Primitives.StringValues values;

            if (_context.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!IsNullOrWhitespace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        private List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }

        private bool IsNullOrWhitespace(string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }

        protected GeoLocationModel getGeoLocation(string ip)
        {
            //ip = "46.101.89.227";//ip is hard coded for testing, its currently UK's IP
            GeoLocationModel locationDetails = new GeoLocationModel();
            using (var client = new HttpClient())
            {
                var response = client.GetStringAsync("https://api.ipgeolocation.io/ipgeo?apiKey=d7b02142c2334f5fb126a04c7807d4c6&ip=" + ip).Result;
                locationDetails = JsonConvert.DeserializeObject<GeoLocationModel>(response);
            }
            return locationDetails;
        }
    }
}
