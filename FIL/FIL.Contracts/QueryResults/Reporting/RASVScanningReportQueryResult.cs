using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Reporting
{
    public class RASVScanningReportQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.Transaction> Transaction { get; set; }
        public IEnumerable<TransactionDetail> TransactionDetail { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<EventAttribute> EventAttribute { get; set; }
        public List<Event> Event { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<ScanningDetailModel> MatchSeatTicketDetail { get; set; }
        public IEnumerable<FIL.Contracts.Models.ReportingColumn> ReportColumns { get; set; }
    }
}