using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.DTCM.OrderResponse
{
    public class OrderResponse
    {
        public string Id { get; set; }
        public string DateTime { get; set; }
        public List<Payment> Payments { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

    public class InventorySource
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Payment
    {
        public string Id { get; set; }
        public InventorySource InventorySource { get; set; }
    }

    public class InventorySource2
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Customer
    {
        public int? Id { get; set; }
        public string AFile { get; set; }
        public string Account { get; set; }
        public int? OrgCustomerId { get; set; }
    }

    public class Fee
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public int SheetId { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public int Total { get; set; }
        public string Bucket { get; set; }
        public bool Inside { get; set; }
        public string FinanceCode { get; set; }
    }

    public class Price
    {
        public int? Id { get; set; }
        public int SheetId { get; set; }
        public int Net { get; set; }
        public List<Fee> Fees { get; set; }
    }

    public class Seat
    {
        public string Section { get; set; }
        public string Row { get; set; }
        public string Seats { get; set; }
        public string RzStr { get; set; }
    }

    public class OrderLineItem
    {
        public int Id { get; set; }
        public string DateTime { get; set; }
        public string PerformanceCode { get; set; }
        public int? PriceCategoryId { get; set; }
        public string PriceCategoryCode { get; set; }
        public string PriceCategoryName { get; set; }
        public string Seller { get; set; }
        public string Channel { get; set; }
        public int? PriceTypeId { get; set; }
        public string PriceTypeCode { get; set; }
        public string PriceTypeName { get; set; }
        public string OfferCode { get; set; }
        public string QualifierCode { get; set; }
        public string Entitlement { get; set; }
        public bool Concession { get; set; }
        public string Barcode { get; set; }
        public Customer Customer { get; set; }
        public Price Price { get; set; }
        public Seat Seat { get; set; }
    }

    public class Fee2
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public int SheetId { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public int Total { get; set; }
        public string Bucket { get; set; }
        public bool Inside { get; set; }
        public object FinanceCode { get; set; }
    }

    public class OrderItem
    {
        public string Id { get; set; }
        public string DateTime { get; set; }
        public InventorySource2 InventorySource { get; set; }
        public List<OrderLineItem> OrderLineItems { get; set; }
        public List<Fee2> Fees { get; set; }
    }
}