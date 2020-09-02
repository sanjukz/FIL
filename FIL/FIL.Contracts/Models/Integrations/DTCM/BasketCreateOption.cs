using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.DTCM.BasketCreateOptions
{
    public class BasketCreateOption
    {
        public string BasketId { get; set; }
        public string Channel { get; set; }
        public string Seller { get; set; }
        public string Performancecode { get; set; }
        public string Area { get; set; }
        public string holdcode { get; set; }
        public List<Demand> Demand { get; set; }
        public List<Fee> Fees { get; set; }
    }

    public class Demand
    {
        public string PriceCategoryCode { get; set; }
        public string PriceTypeId { get; set; }
        public string PriceTypeCode { get; set; }
        public int Quantity { get; set; }
        public int Admits { get; set; }
        public Customer Customer { get; set; }
    }

    public class Customer
    {
    }

    public class Fee
    {
        public string Type { get; set; }
        public string Code { get; set; }
    }
}