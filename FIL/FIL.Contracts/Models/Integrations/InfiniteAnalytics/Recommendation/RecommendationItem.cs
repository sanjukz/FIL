using Newtonsoft.Json;

namespace FIL.Contracts.Models.Integrations.InfiniteAnalytics.Recommendation
{
    public class RecommendationItem
    {
        [JsonProperty("quaternary_category")]
        public string QuaternaryCategory { get; set; }

        [JsonProperty("marketplace")]
        public string Marketplace { get; set; }

        [JsonProperty("keywords")]
        public string Keywords { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("secondary_category")]
        public string SecondaryCategory { get; set; }

        [JsonProperty("availability_status")]
        public bool AvailabilityStatus { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("tertiary_category")]
        public string TertiaryCategory { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_category")]
        public string ParentCategory { get; set; }

        [JsonProperty("site_product_id")]
        public long SiteProductId { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}