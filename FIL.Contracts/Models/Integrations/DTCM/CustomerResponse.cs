namespace FIL.Contracts.Models.Integrations.DTCM
{
    public class CustomerResponse
    {
        public int ID { get; set; }
        public int Account { get; set; }
        public string AFile { get; set; }
        public string CreatedDateTime { get; set; }
        public int OrganisationCustomerID { get; set; }
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