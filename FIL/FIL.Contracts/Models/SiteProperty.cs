using System;

namespace FIL.Contracts.Models
{
    public class SiteProperty
    {
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string GoogleSiteVerification { get; set; }
        public string HrefLang { get; set; }
        public string Keyword { get; set; }
    }
}