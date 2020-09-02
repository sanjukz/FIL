namespace FIL.Contracts.Models.Integrations.DTCM
{
    public class CustomerCreateOption
    {
        public string ID { get; set; }
        public string SellerCode { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string InternationalCode { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string CountryCode { get; set; }
    }
}