namespace FIL.Contracts.Models.Integrations.InfiniteAnalytics.Recommendation
{
    public class RecommendationModel
    {
        public string SessionId { get; set; }
        public string ClientType { get; set; }
        public string SitePageType { get; set; }
        public string siteProductId { get; set; }
        public int Count { get; set; }
    }
}