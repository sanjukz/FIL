using System;

namespace FIL.Contracts.Models.Integrations.ValueRetail
{
    public class BookCartResponse
    {
        public int JobId { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string VillageCode { get; set; }
        public string CultureCode { get; set; }
        public decimal Pricing { get; set; }
        public string Id { get; set; }
        public string ReferenceURL { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}