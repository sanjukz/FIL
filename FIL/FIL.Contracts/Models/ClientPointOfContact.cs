using System;

namespace FIL.Contracts.Models
{
    public class ClientPointOfContact
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}