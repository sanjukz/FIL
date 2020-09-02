using System;

namespace FIL.Contracts.Models.CreateEventV1
{
    public class EventFinanceDetailModel
    {
        public int Id { get; set; }
        public FIL.Contracts.Enums.AccountType AccountTypeId { get; set; }
        public int CountryId { get; set; }
        public int CurrenyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }
        public int StateId { get; set; }
        public string TaxId { get; set; }
        public long EventId { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}