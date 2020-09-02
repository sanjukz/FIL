using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.Enums;
using FIL.Web.Core.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Checkout
{
    public class LoginTransactionFormDataViewModel
    {
        public LoginFormDataViewModel UserDetail { get; set; }
        public List<EventTicketAttribute> EventTicketAttributeList { get; set; }
        public bool IsItinerary { get; set; }
        public bool IsTiqets { get; set; }
        public string TransactionCurrency { get; set; }
        public decimal? DonationAmount { get; set; }
        public string ReferralId { get; set; }
    }
}
