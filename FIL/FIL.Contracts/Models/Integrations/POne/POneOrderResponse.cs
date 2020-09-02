using Newtonsoft.Json;

namespace FIL.Contracts.Models.Integrations.POne
{
    public class POneOrderResponse
    {
        [JsonProperty("message")]
        public string message { get; set; }

        [JsonProperty("status_code")]
        public int status_code { get; set; }

        [JsonProperty("products_failed")]
        public object products_failed { get; set; }

        [JsonProperty("orderNr")]
        public string orderNr { get; set; }
    }
}