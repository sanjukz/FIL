using FIL.Contracts.Commands.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Checkout
{
    public class GuestSignInToDeliveryTransactionFormDataViewModel
    {
        public Guid UserAltId { get; set; }
        public List<EventTicketAttribute> EventTicketAttributeList { get; set; }
        public bool IsItinerary { get; set; }
        public string TransactionCurrency { get; set; }
        public bool IsTiqets { get; set; }
        public decimal? DonationAmount { get; set; }
        public string ReferralId { get; set; }
        public bool IsBSPUpgrade { get; set; }
    }
}
