using System.Collections.Generic;

namespace FIL.Contracts.DataModels
{
    public class Reporting
    {
        public IEnumerable<Transaction> Transaction { get; set; }
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
        public IEnumerable<Country> Country { get; set; }
        public IEnumerable<Event> Event { get; set; }
        public IEnumerable<User> User { get; set; }
        public IEnumerable<UserCardDetail> UserCardDetail { get; set; }
        public IEnumerable<IPDetail> IPDetail { get; set; }
        public IEnumerable<TicketFeeDetail> TicketFeeDetail { get; set; }
        public IEnumerable<ReportingColumn> ReportColumns { get; set; }
        public IEnumerable<ReportingColumnsUserMapping> ReportingColumnsUserMapping { get; set; }
        public IEnumerable<ReportingColumnsMenuMapping> ReportingColumnsMenuMapping { get; set; }
        public IEnumerable<FIL.Contracts.Models.ScanningDetailModel> MatchSeatTicketDetail { get; set; }
        public IEnumerable<FIL.Contracts.Models.BOCustomerDetail> BOCustomerDetail { get; set; }
    }
}