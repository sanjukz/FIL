using System.Collections.Generic;

namespace FIL.Contracts.Models.Tiqets
{
    public class Pagination
    {
        public int total { get; set; }
        public int page { get; set; }
        public int page_size { get; set; }
    }

    public class Geolocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Image
    {
        public string small { get; set; }
        public string large { get; set; }
        public string medium { get; set; }
        public string credits { get; set; }
    }

    public class Ratings
    {
        public double? average { get; set; }
        public int total { get; set; }
    }

    public class Venue
    {
        public string address { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Product
    {
        public Geolocation geolocation { get; set; }
        public string city_id { get; set; }
        public string country_id { get; set; }
        public string product_url { get; set; }
        public IList<Image> images { get; set; }
        public string id { get; set; }
        public Ratings ratings { get; set; }
        public string whats_included { get; set; }
        public string title { get; set; }
        public string sale_status { get; set; }
        public IList<string> tag_ids { get; set; }
        public IList<string> languages { get; set; }
        public string country_name { get; set; }
        public double price { get; set; }
        public string sale_status_reason { get; set; }
        public string city_name { get; set; }
        public string product_slug { get; set; }
        public string language { get; set; }
        public Venue venue { get; set; }
        public string summary { get; set; }
        public string whats_excluded { get; set; }
        public string tagline { get; set; }
        public string promo_label { get; set; }
        public string product_checkout_url { get; set; }
        public double? prediscount_price { get; set; }
    }

    public class AllProductResponseModel
    {
        public bool success { get; set; }
        public Pagination pagination { get; set; }
        public IList<Product> products { get; set; }
    }
}