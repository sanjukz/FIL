using System;

namespace FIL.Contracts.Commands.FinanceDetails
{
    public class FinancDetailCommand : Contracts.Interfaces.Commands.ICommandWithResult<FinancDetailCommandResult>
    {
        public int Id { get; set; }
        public long CurrencyId { get; set; }
        public long CountryId { get; set; }
        public long StateId { get; set; }
        public long EventDetailId { get; set; }
        public string AccountType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccountType { get; set; }
        public bool IsBankAccountGST { get; set; }
        public string BankName { get; set; }
        public int BankStateId { get; set; }
        public string RoutingNo { get; set; }
        public string GSTNo { get; set; }
        public string AccountNo { get; set; }
        public string PANNo { get; set; }
        public string AccountNickName { get; set; }
        public bool IsUpdatLater { get; set; }
        public string FinancialsAccountBankAccountGSTInfo { get; set; }
        public string Placename { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public long EventId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class FinancDetailCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}