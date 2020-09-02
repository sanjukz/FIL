using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FIL.ExOzConsoleApp.Entities.Classes
{
    public class Operator
    {
        public int id { get; set; }
        public string name { get; set; }
        public string publicName { get; set; }
        public string urlSegment { get; set; }
        public string summary { get; set; }
        public int regionId { get; set; }
        public int EventCatId { get; set; }
        public int VenueId { get; set; }
        public string canonicalRegionUrlSegment { get; set; }
        public string hotDealMessage { get; set; }
        public string hotDealExpiryDate { get; set; }
        public double fromPrice { get; set; }
        public double retailPrice { get; set; }
        public string levy { get; set; }
        public string rating { get; set; }
        public string tips { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public List<Geolocation> geolocations { get; set; }
        public int quantity { get; set; }
        public List<ExOzProduct> products { get; set; }
        public List<string> categoryIds { get; set; }
        public string timestamp { get; set; }
        public string[] images { get; set; }

        public Operator()
        {
            this.id = 0;
            this.name = string.Empty;
            this.publicName = string.Empty;
            this.urlSegment = string.Empty;
            this.summary = string.Empty;
            this.regionId = 0;
            this.EventCatId = 0;
            this.VenueId = 0;
            this.canonicalRegionUrlSegment = string.Empty;
            this.hotDealMessage = string.Empty;
            this.hotDealExpiryDate = string.Empty;
            this.fromPrice = 0;
            this.retailPrice = 0;
            this.levy = string.Empty;
            this.rating = string.Empty;
            this.tips = string.Empty;
            this.title = string.Empty;
            this.description = string.Empty;
            this.address = string.Empty;
            this.phone = string.Empty;
            this.quantity = 0;
            this.timestamp = string.Empty;
        }
    }


}
