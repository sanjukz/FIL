using System;

namespace FIL.Contracts.Models
{
    public class SiteBannerDetail
    {
        public Guid AltId { get; set; }
        public string BannerName { get; set; }
        public short SortOrder { get; set; }
    }
}