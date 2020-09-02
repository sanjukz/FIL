using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TicketLookup
{
    public class TicketLookupQueryResult
    {
        public FIL.Contracts.Models.Transaction Transaction { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string PaymentOption { get; set; }
        public string PayconfigNumber { get; set; }
        public List<TicketLookupSubContainer> TicketLookupSubContainer { get; set; }
    }
}