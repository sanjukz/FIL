using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.CurrentOrderData
{
    public class CurrentOrderDataQueryResult
    {
        public FIL.Contracts.Models.Transaction Transaction { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public List<OrderConfirmationSubContainer> orderConfirmationSubContainer { get; set; }
        public int TicketQuantity { get; set; }
    }
}