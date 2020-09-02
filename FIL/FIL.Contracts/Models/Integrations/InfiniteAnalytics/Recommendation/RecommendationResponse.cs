using Newtonsoft.Json;
using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.InfiniteAnalytics.Recommendation
{
    public class RecommendationResponse
    {
        [JsonProperty("data")]
        public List<RecommendationData> Data { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}