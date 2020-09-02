using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Reporting
{
    public class FailedTransactionReportQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.Transaction> Transaction { get; set; }
        public IEnumerable<TransactionDetail> TransactionDetail { get; set; }
        public IEnumerable<TransactionDeliveryDetail> TransactionDeliveryDetail { get; set; }
        public IEnumerable<TransactionPaymentDetail> TransactionPaymentDetail { get; set; }
        public IEnumerable<CurrencyType> CurrencyType { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<EventAttribute> EventAttribute { get; set; }
        public IEnumerable<Venue> Venue { get; set; }
        public IEnumerable<City> City { get; set; }
        public IEnumerable<State> State { get; set; }
        public IEnumerable<FIL.Contracts.Models.Country> Country { get; set; }
        public IEnumerable<Event> Event { get; set; }
        public IEnumerable<FIL.Contracts.Models.User> User { get; set; }
        public IEnumerable<FIL.Contracts.Models.UserCardDetail> UserCardDetail { get; set; }
        public IEnumerable<FIL.Contracts.Models.ReportingColumn> ReportColumns { get; set; }
        public IEnumerable<FIL.Contracts.Models.IPDetail> IPDetail { get; set; }
    }
}