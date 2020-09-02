namespace FIL.Contracts.Models.TMS
{
    public class TicketHandoverDetail
    {
        public string EventName { get; set; }
        public string EventDetailName { get; set; }
        public string VenueName { get; set; }
        public string CityName { get; set; }
        public string EventStartDate { get; set; }
        public string EventStartTime { get; set; }
        public string TotalTickets { get; set; }
        public string TicketCategory { get; set; }
        public string CurrencyName { get; set; }
        public decimal PricePerTicket { get; set; }
        public decimal DiscountAmount { get; set; }
        public string TransactionType { get; set; }
        public string SponsorName { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrencyId { get; set; }
        public string CreatedUtc { get; set; }
    }
}