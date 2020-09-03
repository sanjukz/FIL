using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Window.Service.Models
{
    public class TiqetProduct
    {
        public int id { get; set; }
        public string productId { get; set; }
        public string tittle { get; set; }
        public string saleStatus { get; set; }
        public string inclusions { get; set; }
        public string language { get; set; }
        public string countryName { get; set; }
        public string cityName { get; set; }
        public string productSlug { get; set; }
        public double price { get; set; }
        public string saleStatuReason { get; set; }
        public string venueName { get; set; }
        public string venueAddress { get; set; }
        public string summary { get; set; }
        public string tagLine { get; set; }
        public string promoLabel { get; set; }
        public string ratingAverage { get; set; }
        public string geoLocationLatitude { get; set; }
        public string geoLocationLongitude { get; set; }
        public bool isEnabled { get; set; }
    }

    public class ProductResponse
    {
        public int id { get; set; }
        public IList<TiqetProduct> tiqetProducts { get; set; }
    }

    public class ProductResponseModel
    {
        public ProductResponse productResponse { get; set; }
        public bool success { get; set; }
        public int remainingProducts { get; set; }
    }

    public class ProductUpdateResponseModel
    {
        public bool success { get; set; }
    }
}
