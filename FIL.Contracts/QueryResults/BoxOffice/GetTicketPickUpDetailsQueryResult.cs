using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class GetTicketPickUpDetailsQueryResult
    {
        public FIL.Contracts.Models.Transaction Transaction { get; set; }
        public FIL.Contracts.Models.User User { get; set; }
        public TransactionPaymentDetail TransactionPaymentDetail { get; set; }
        public UserCardDetail UserCardDetail { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string PaymentOption { get; set; }
        public string cardTypes { get; set; }
        public List<FIL.Contracts.Models.BoxOffice.EventContainer> eventsContainer { get; set; }
        public int TicketQuantity { get; set; }
    }
}