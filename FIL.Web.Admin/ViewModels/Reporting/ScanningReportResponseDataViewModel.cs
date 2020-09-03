using System.Collections.Generic;
using FIL.Contracts.Models;

namespace FIL.Web.Admin.ViewModels.Reporting
{
    public class ScanningReportResponseDataViewModel
    {
        public List<FIL.Contracts.Models.Event> Event { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<EventAttribute> EventAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<TransactionDetail> TransactionDetail { get; set; }
        public IEnumerable<FIL.Contracts.Models.Transaction> Transaction { get; set; }
        public IEnumerable<ScanningDetailModel> MatchSeatTicketDetail { get; set; }
        public IEnumerable<ReportingColumn> ReportColumns { get; set; }
    }
}
