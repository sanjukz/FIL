namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IBillingAddress
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string PhoneCode { get; set; }
        string PhoneNumber { get; set; }
        string Email { get; set; }
        string AddressLine1 { get; set; }
        string AddressLine2 { get; set; }
        string Zipcode { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Country { get; set; }
    }

    public class BillingAddress : IBillingAddress
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}