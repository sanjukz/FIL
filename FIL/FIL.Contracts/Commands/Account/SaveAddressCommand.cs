using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Commands.Account
{
    public class SaveAddressCommand : BaseCommand
    {
        public Guid UserAltId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int Zipcode { get; set; }
        public AddressTypes? AddressTypeId { get; set; }
    }
}