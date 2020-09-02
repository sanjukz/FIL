using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class TicketLookupEmailDetailContainer
    {
        public Transaction Transaction { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string PaymentOption { get; set; }
        public string PayconfigNumber { get; set; }
        public List<TicketLookupSubContainer> TicketLookupSubContainer { get; set; }
    }
}