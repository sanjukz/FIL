using Newtonsoft.Json;
using System.Collections.Generic;

namespace FIL.Contracts.Models.POne
{
    public class Language
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("category")]
        public string category { get; set; }

        [JsonProperty("sub_category")]
        public string sub_category { get; set; }

        [JsonProperty("sub_category_full")]
        public IList<string> sub_category_full { get; set; }

        [JsonProperty("location")]
        public string location { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("event")]
        public string @event { get; set; }

        [JsonProperty("event_guest")]
        public string event_guest { get; set; }

        [JsonProperty("seating")]
        public string seating { get; set; }

        [JsonProperty("seating_id")]
        public string seating_id { get; set; }

        [JsonProperty("match_id")]
        public string match_id { get; set; }

        [JsonProperty("delivery_description")]
        public string delivery_description { get; set; }

        [JsonProperty("seating_description")]
        public string seating_description { get; set; }

        [JsonProperty("hospitality_description")]
        public string hospitality_description { get; set; }
    }

    public class Languages
    {
        public Language NL { get; set; }
        public Language EN { get; set; }
        public Language DE { get; set; }
        public Language IT { get; set; }
        public Language FR { get; set; }
        public Language ES { get; set; }
    }

    public class SkuModel
    {
        [JsonProperty("sku")]
        public string sku { get; set; }

        [JsonProperty("event_id")]
        public string event_id { get; set; }

        [JsonProperty("price")]
        public string price { get; set; }

        [JsonProperty("stock")]
        public string stock { get; set; }

        [JsonProperty("shipping_method")]
        public string shipping_method { get; set; }

        [JsonProperty("shipping_costs")]
        public string shipping_costs { get; set; }

        [JsonProperty("date")]
        public string date { get; set; }

        [JsonProperty("time")]
        public string time { get; set; }

        [JsonProperty("datetime_confirmed")]
        public string datetime_confirmed { get; set; }

        [JsonProperty("image")]
        public string image { get; set; }

        [JsonProperty("lat")]
        public string lat { get; set; }

        [JsonProperty("lng")]
        public string lng { get; set; }

        [JsonProperty("formatted_address")]
        public string formatted_address { get; set; }

        [JsonProperty("languages")]
        public Languages languages { get; set; }
    }
}