using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FIL.MailChimp.Models;
using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Models;
using Newtonsoft.Json;

namespace FIL.MailChimp
{
    public static class ListExtension
    {
        public static GeoLocationModel GetLocationByIp(string ip)
        {
            GeoLocationModel locationDetails = new GeoLocationModel();
            using (var client = new HttpClient())
            {
                var response = client.GetStringAsync("https://api.ipgeolocation.io/ipgeo?apiKey=d7b02142c2334f5fb126a04c7807d4c6&ip=" + ip).Result;
                locationDetails = JsonConvert.DeserializeObject<GeoLocationModel>(response);
            }
            return locationDetails;
        }
    }
}