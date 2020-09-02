using FIL.Logging;
using FIL.Configuration;
using System;
using Newtonsoft.Json;
using static FIL.Web.Feel.Controllers.PlaceItinerarySearchController;
using System.Net;
using System.Text;

namespace FIL.Web.Feel.Providers
{
    public interface IVisitDurationProvider
    {
        Parent GetDistance(string originAddress, string destinationAddress);
    }

    public class VisitDurationProvider : IVisitDurationProvider
    {
        private readonly FIL.Logging.ILogger _logger;
        public VisitDurationProvider(ILogger logger, ISettings settings
            )
        {
            _logger = logger;
        }

        public Parent GetDistance(string originAddress, string destinationAddress)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create("https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + originAddress + "&destinations=" + destinationAddress + "&mode=driving&language=en-EN&sensor=false&key=AIzaSyDZ4Ik7ENzLWY1tLh1ul8NxhWBdWGK6tQU") as HttpWebRequest;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                WebHeaderCollection header = response.Headers;
                var encoding = ASCIIEncoding.ASCII;
                Parent result = new Parent();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    string responseText = reader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<Parent>(responseText);
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return null;
            }
        }
    }
}
