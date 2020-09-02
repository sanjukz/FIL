using System;

namespace FIL.Contracts.Models.ASI
{
    public class ASITicketInfo
    {
        public string QRCode { get; set; }
        public string EventName { get; set; }
        public string TicketCategory { get; set; }
        public string TicketType { get; set; }
        public DateTime VisitDate { get; set; }
        public string VenueName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryeName { get; set; }
        public decimal Price { get; set; }
        public string VisitTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long TransactionId { get; set; }
        public string TicketNo { get; set; }
    }
}