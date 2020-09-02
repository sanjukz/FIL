using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Redemption
{
    public class MasterFinanceDetails : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int CurrenyId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public Contracts.Enums.AccountType AccountTypeId { get; set; }
        public Contracts.Enums.BankAccountType BankAccountTypeId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string BranchCode { get; set; }
        public string TaxId { get; set; }
        public string RoutingNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountName { get; set; }
        public long EventId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}