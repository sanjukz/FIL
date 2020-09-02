using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class TicketLookupUserCardDetailContainer
    {
        public Transaction Transaction { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string PaymentOption { get; set; }
        public List<TicketLookupSubContainer> TicketLookupSubContainer { get; set; }
    }
}