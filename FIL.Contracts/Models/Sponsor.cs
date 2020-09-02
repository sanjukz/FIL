using FIL.Contracts.Enums;

namespace FIL.Contracts.Models
{
    public class Sponsor
    {
        public long Id { get; set; }
        public string SponsorName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string CityId { get; set; }
        public string StateId { get; set; }
        public string CountryId { get; set; }
        public string IdType { get; set; }
        public string IdNumber { get; set; }
        public SponsorType SponsorTypeId { get; set; }
        public bool IsEnabled { get; set; }
    }
}