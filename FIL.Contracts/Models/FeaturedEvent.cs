namespace FIL.Contracts.Models
{
    public class FeaturedEvent
    {
        public long EventId { get; set; }
        public bool IsEnabled { get; set; }
        public short SortOrder { get; set; }
        public Enums.Site SiteId { get; set; }
        public bool? IsAllowedInFooter { get; set; }
    }
}