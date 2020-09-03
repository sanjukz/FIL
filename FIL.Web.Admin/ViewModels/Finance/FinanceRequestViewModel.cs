using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Finance
{
    public class FinanceRequestViewModel
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string AccountType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAcountType { get; set; }
        public bool IsBankAccountGST { get; set; }
        public string BankName { get; set; }
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
        public int EventId { get; set; }
        public int EventDetailId { get; set; }
        public int AltId { get; set; }
    }
}

   

