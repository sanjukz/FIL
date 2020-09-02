namespace FIL.Contracts.Models.TMS
{
    public class CorporateTicketAllocationDetail
    {
        public long Id { get; set; }
        public string SponsorName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
        public long SponsorId { get; set; }
        public long EventDetailId { get; set; }
        public string EventName { get; set; }
        public string EventStartDate { get; set; }
        public string EventStartTime { get; set; }
        public long EventId { get; set; }
        public string EventDetailName { get; set; }
        public string VenueName { get; set; }
        public string CityName { get; set; }
        public string TicketCategory { get; set; }
        public decimal PricePerTicket { get; set; }
        public long EventTicketAttributeId { get; set; }
        public string ConvenceCharge { get; set; }
        public string ServiceTax { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyId { get; set; }
    }
}