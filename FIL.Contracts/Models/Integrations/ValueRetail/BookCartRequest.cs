using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.ValueRetail
{
    public class ValueRetailCustomer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string SecondaryEmail { get; set; }
        public string Phone { get; set; }
        public object MiddleName { get; set; }
        public string CompanyName { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public DateTime DOB { get; set; }
        public bool MarketingFlag { get; set; }
        public string AddressLine { get; set; }
        public string AddressLine2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Building { get; set; }
        public int CountryCode { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public string MobileCountryCode { get; set; }
        public int SecondaryMobileCountryCode { get; set; }
        public long SecondaryMobile { get; set; }
    }

    public class PrivateData
    {
        public IList<object> GiftCards { get; set; }
    }

    public class BookCartRequest
    {
        public string Id { get; set; }
        public ValueRetailCustomer Customer { get; set; }
        public string Hash { get; set; }
        public object CardTypes { get; set; }
        public PrivateData PrivateData { get; set; }
        public string AggregatorId { get; set; }
        public string OtaId { get; set; }
    }
}