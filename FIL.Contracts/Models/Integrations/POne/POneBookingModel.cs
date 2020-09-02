using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.POne
{
    public class Product
    {
        public string sku { get; set; }
        public int amount { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phonenumber { get; set; }
        public string booking_reference { get; set; }
        public string checkin_date { get; set; }
        public string street { get; set; }
        public int housenumber { get; set; }
        public string zipcode { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string passport_name { get; set; }
        public string date_of_birth { get; set; }
        public string nationality { get; set; }
    }

    public class POneBookingModel
    {
        public IList<Product> products { get; set; }
    }
}