namespace FIL.Contracts.Models.TMS
{
    public class CartData
    {
        public string Name { get; set; }
        public string startDateTime { get; set; }
        public string venueName { get; set; }
        public string city { get; set; }
        public string ticketCategoryName { get; set; }
        public decimal price { get; set; }
        public string currencyCode { get; set; }
        public string deliveryType { get; set; }
    }
}