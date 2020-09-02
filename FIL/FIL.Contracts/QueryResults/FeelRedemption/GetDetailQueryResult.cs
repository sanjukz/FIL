using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.FeelRedemption
{
    public class GetDetailQueryResult
    {
        public List<EventTicketAttribute> EventTicketAttribute { get; set; }
        public List<TransactionDetail> TransactionDetail { get; set; }
        public List<EventTicketDetail> EventTicketDetail { get; set; }
        public List<TicketCategory> TicketCategory { get; set; }
        public List<EventDetail> EventDetail { get; set; }
    }
}