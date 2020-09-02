namespace FIL.Contracts.Models.TMS
{
    public class CorporateOrderDetails
    {
        public long TransactionId { get; set; }
        public string PaymentType { get; set; }
        public string EventName { get; set; }
        public string EventDetailName { get; set; }
        public string VenueName { get; set; }
        public string CityName { get; set; }
        public string EventStartDate { get; set; }
        public string TransactionType { get; set; }
        public long VenueId { get; set; }
        public string EventStartTime { get; set; }
        public string PricePerTicket { get; set; }
        public string TicketCategoryName { get; set; }
        public long EventTicketAttributeId { get; set; }
        public int TotalTickets { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyId { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal ConvenienceCharges { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetTicketAmount { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
        public string CreatedUtc { get; set; }
    }
}