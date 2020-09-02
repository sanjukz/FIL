using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIL.ExOzConsoleApp
{
    public static class Mapper<T>
    {
        public static List<T> MapCollectionFromJson(string json, string token = "data")
        {
            var list = new List<T>();

            var jObject = JObject.Parse(json);

            var allTokens = jObject.SelectToken(token);
            foreach (var tkn in allTokens)
                list.Add(MapFromJson(tkn.ToString()));
            return list;
        }

        public static T MapFromJson(string json, string parentToken = null)
        {
            var jsonToParse = string.IsNullOrWhiteSpace(parentToken) ? json : JObject.Parse(json).SelectToken(parentToken).ToString();

            return JsonConvert.DeserializeObject<T>(jsonToParse);
        }

        public static T ExecuteWebRequest(string input)
        {
            string postResponse = HttpWebRequestHelper.ExOz_WebRequestGet(input);
            var objDetails = Mapper<T>.MapFromJson(postResponse);
            if (postResponse == "0")
            {
                // TODO: Error
            }
            return objDetails;
        }
    }
}
