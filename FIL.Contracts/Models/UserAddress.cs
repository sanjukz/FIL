using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class UserAddress
    {
        public Guid AltId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string Zipcode { get; set; }
        public AddressTypes? AddressTypeId { get; set; }
        public bool? IsDefault { get; set; }
    }
}