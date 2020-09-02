using Newtonsoft.Json;

namespace FIL.Contracts.Models.Integrations.InfiniteAnalytics
{
    public class SessionResponse
    {
        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("client_ip")]
        public string ClientIp { get; set; }
    }
}