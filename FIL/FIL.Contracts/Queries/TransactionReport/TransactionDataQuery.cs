using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TransactionReport;
using System;

namespace FIL.Contracts.Queries.TransactionReport
{
    public class TransactionDataQuery : IQuery<TransactionDataQueryResult>
    {
        public string EventAltId { get; set; }
        public Guid UserAltId { get; set; }
        public string EventDetailId { get; set; }
        public string ChannelId { get; set; }
        public string VenueId { get; set; }
        public string TicketCategoryId { get; set; }
        public string TicketTypes { get; set; }
        public string CurrencyTypes { get; set; }
        public string TransactionTypes { get; set; }
        public string PaymentGatewayes { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}