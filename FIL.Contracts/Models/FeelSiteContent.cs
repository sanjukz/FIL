using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class FeelSiteContent
    {
        public Guid AltId { get; set; }
        public string SiteTitle { get; set; }
        public string SiteLogo { get; set; }
        public string BannerText { get; set; }
        public SiteLevel SiteLevel { get; set; }
    }
}