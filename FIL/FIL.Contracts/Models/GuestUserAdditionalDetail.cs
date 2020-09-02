using System;

namespace FIL.Contracts.Models
{
    public class GuestUserAdditionalDetail
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Guid AltId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}