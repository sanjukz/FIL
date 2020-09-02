using FIL.Contracts.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Web.Core.UrlsProvider
{
    public interface ISiteUrlsProvider
    {
        string GetSiteUrl(Site site);
    }

    public class SiteUrlsProvider : ISiteUrlsProvider
    {
        public string GetSiteUrl(Site site)
        {
            switch (site)
            {
                case Site.AeSite:
                    return "https://www.kyazoonga.ae";
                case Site.AuSite:
                    return "https://www.zoonga.com.au";
                case Site.ComSite:
                    return "https://www.zoonga.com";
                case Site.feelaplaceSite:
                    return "https://www.feelitlive.com";
                case Site.feelDubaiSite:
                    return "https://www.feeldubai.com";
                case Site.feelIndiaSite:
                    return "https://www.feelindia.com";
                case Site.feelMadhyaPradeshSite:
                    return "https://www.feelmadhyapradesh.com";
                case Site.feelMaharashtraSite:
                    return "https://www.feelmaharashtra.com";
                case Site.feelPondicherrySite:
                    return "https://www.feelpondicherry.com";
                case Site.feelRajasthanSite:
                    return "https://www.feelrajasthan.com";
                case Site.feelUttarPradeshSite:
                    return "https://www.feeluttarpradesh.com";
                case Site.feelNewYork:
                    return "https://www.feelnewyork.com";
                case Site.feelLondon:
                    return "https://www.feellondon.com";
                case Site.IeSite:
                    return "https://www.kyazoonga.ie";
                case Site.MobileApp:
                    return "https://www.zoonga.com";
                case Site.None:
                    return "https://www.zoonga.com";
                case Site.RASVSite:
                    return "https://tickets.royalshow.com.au";
                case Site.UkSite:
                    return "https://www.zoonga.co.uk";
                case Site.IccWWCT20:
                    return "https://wwt202018.kyazoonga.com";
                case Site.feelAntiguaandBarbuda:
                    return "https://www.feelantiguaandbarbuda.com";
                case Site.feelantigua:
                    return "https://www.feelantigua.com";
                case Site.feelthecaribbean:
                    return "https://www.feelthecaribbean.com";
                case Site.feelstlucia:
                    return "https://www.feelstlucia.com";
                case Site.feelsaintlucia:
                    return "https://www.feelsaintlucia.com";
                case Site.DevelopmentSite:
                    return "https://webdev.kyazoonga.com";
                case Site.Ace:
                    return "https://ausopen.zoonga.com";
                case Site.Mopt:
                    return "https://mopt.zoonga.com";
                case Site.FeelDevelopmentSite:
                    return "https://dev.feelitlive.com";
                default:
                    return "https://www.zoonga.com";
            }
        }

    }
}