using System;

namespace FIL.Contracts.Models.DynamicContent
{
    public class GetTicketDetailResponseModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public DateTime SalesStartDateTime { get; set; }
        public DateTime SalesEndDateTime { get; set; }
        public long AvailableTicketForSale { get; set; }
    }
}