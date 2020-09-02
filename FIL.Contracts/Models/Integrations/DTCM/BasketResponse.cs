using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.DTCM.BasketResponse
{
    public class BasketResponse
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public List<Offer> Offers { get; set; }
        public List<object> Fees { get; set; }
    }

    public class Customer
    {
        public int? Id { get; set; }
        public string AFile { get; set; }
        public int? Account { get; set; }
        public int? OrgCustomerId { get; set; }
    }

    public class Fee
    {
        public int? Id { get; set; }
        public string Bucket { get; set; }
        public string Code { get; set; }
        public string FinanceCode { get; set; }
        public bool Inside { get; set; }
        public string Name { get; set; }
        public int SheetId { get; set; }
        public int Total { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
    }

    public class Price
    {
        public int? Id { get; set; }
        public int SheetId { get; set; }
        public int Net { get; set; }
        public List<Fee> Fees { get; set; }
    }

    public class Demand
    {
        public int? PriceTypeId { get; set; }
        public string PriceTypeCode { get; set; }
        public string PriceType { get; set; }
        public int Quantity { get; set; }
        public int Admits { get; set; }
        public string OfferCode { get; set; }
        public string QualifierCode { get; set; }
        public string Entitlement { get; set; }
        public Customer Customer { get; set; }
        public List<Price> Prices { get; set; }
    }

    public class Seat
    {
        public string Section { get; set; }
        public string Row { get; set; }
        public string Seats { get; set; }
        public string RzStr { get; set; }
    }

    public class Fee2
    {
        public int? Id { get; set; }
        public string Bucket { get; set; }
        public string Code { get; set; }
        public string FinanceCode { get; set; }
        public bool Inside { get; set; }
        public string Name { get; set; }
        public int SheetId { get; set; }
        public int Total { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
    }

    public class Offer
    {
        public string Id { get; set; }
        public string InventorySource { get; set; }
        public string DateTime { get; set; }
        public string PerformanceCode { get; set; }
        public int? PriceCategoryId { get; set; }
        public string PriceCategoryCode { get; set; }
        public string PriceCategory { get; set; }
        public string Seller { get; set; }
        public string Channel { get; set; }
        public List<Demand> Demand { get; set; }
        public List<Seat> Seats { get; set; }
        public List<Fee2> Fees { get; set; }
    }
}