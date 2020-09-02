using Newtonsoft.Json;
using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.InfiniteAnalytics.Recommendation
{
    public class RecommendationData
    {
        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("items")]
        public List<RecommendationItem> Items { get; set; }

        [JsonProperty("widget_type")]
        public string WidgetType { get; set; }
    }
}