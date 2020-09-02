using FIL.Contracts.Enums;

namespace FIL.Contracts.Models.TMS
{
    public class SponsorOrderDetail
    {
        public long Id { get; set; }
        public AccountType AccountTypeId { get; set; }
        public OrderType OrderTypeId { get; set; }
        public string SponsorName { get; set; }
        public string Name { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string EventName { get; set; }
        public string EventDetailName { get; set; }
        public string VenueName { get; set; }
        public string CityName { get; set; }
        public string EventStartDate { get; set; }
        public string TicketCategoryName { get; set; }
        public short RequestedTickets { get; set; }
        public short AllocatedTickets { get; set; }
        public decimal LocalPrice { get; set; }
        public string CurrencyName { get; set; }
        public int RemainingTicketForSale { get; set; }
        public OrderStatus OrderStatusId { get; set; }
        public long EventTicketAttributeId { get; set; }
        public InventoryType InventoryTypeId { get; set; }
        public string RequestedBy { get; set; }
    }
}