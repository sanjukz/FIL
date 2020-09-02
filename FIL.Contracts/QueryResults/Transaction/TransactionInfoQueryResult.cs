using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Transaction
{
    public class TransactionInfoQueryResult
    {
        public FIL.Contracts.Models.Transaction Transaction { get; set; }
        public string CurrencyName { get; set; }
        public List<Event> Events { get; set; }
        public TransactionType TransactionType { get; set; }
        public List<EventTicketAttribute> EventTicketAttributes { get; set; }
        public List<TransactionDetail> TransactionDetails { get; set; }
    }
}