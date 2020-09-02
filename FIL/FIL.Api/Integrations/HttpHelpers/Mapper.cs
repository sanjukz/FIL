using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FIL.Api.Integrations.HttpHelpers
{
    public static class Mapper<T>
    {
        public static T MapJsonStringToObject(string json, string parentToken = null)
        {
            var jsonToParse = string.IsNullOrWhiteSpace(parentToken) ? json : JObject.Parse(json).SelectToken(parentToken).ToString();
            return JsonConvert.DeserializeObject<T>(jsonToParse);
        }

        public static string MapObjectToJsonString(dynamic objCreateOptions)
        {
            return JsonConvert.SerializeObject(objCreateOptions);
        }
    }
}