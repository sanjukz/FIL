using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.Redemption
{
    public class GuideDetailsCustom : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Guid AltId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhonceCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PlaceName { get; set; }
        public string VenueName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string LanguageId { get; set; }
        public string ApproveStatusId { get; set; }
        public Guid ApprovedBy { get; set; }
        public DateTime? ApprovedUtc { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string AccountNumber { get; set; }
        public FIL.Contracts.Enums.AccountType AccountType { get; set; }
        public FIL.Contracts.Enums.BankAccountType BankAccountType { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        public string CurrencyName { get; set; }
        public string RoutingNumber { get; set; }
        public string TaxId { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}