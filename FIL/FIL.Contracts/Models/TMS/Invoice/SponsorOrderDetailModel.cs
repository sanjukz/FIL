using FIL.Contracts.Enums;

namespace FIL.Contracts.Models.TMS.Invoice
{
    public class SponsorOrderDetailModel
    {
        public long Id { get; set; }
        public string EventDetailName { get; set; }
        public string TicketCategoryName { get; set; }
        public short Quantity { get; set; }
        public decimal LocalPrice { get; set; }
        public string CurrencyName { get; set; }
        public string ConvenienceCharge { get; set; }
        public string ServiceCharge { get; set; }
        public OrderStatus OrderStatusId { get; set; }
    }
}