namespace FIL.Contracts.Models
{
    public class FinanceDetails
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        public bool AccountType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankName { get; set; }
        public string BankAccountType { get; set; }
        public int BankStateId { get; set; }
        public string RoutingNo { get; set; }
        public string GSTNo { get; set; }
        public string AccountNo { get; set; }
        public string PANNo { get; set; }
        public string AccountNickName { get; set; }
    }
}