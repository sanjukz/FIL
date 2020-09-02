using System;

namespace FIL.Contracts.QueryResult
{
    public class FinancDetailsByIdQueryResults
    {
        public int Id { get; set; }
        public long CurrencyId { get; set; }
        public long CountryId { get; set; }
        public long EventDetailId { get; set; }
        public string AccountType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccountType { get; set; }
        public string BankName { get; set; }
        public long StateId { get; set; }
        public string RoutingNo { get; set; }
        public string GSTNo { get; set; }
        public string AccountNo { get; set; }
        public string PANNo { get; set; }
        public string AccountNickName { get; set; }
        public string placename { get; set; }
        public string location { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string FinancialsAccountBankAccountGSTInfo { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public long EventId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}