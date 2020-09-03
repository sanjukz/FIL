using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Reporting
{
    public class InventoryReportResponseDataViewModel
    { 
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<CorporateTicketAllocationDetail> CorporateTicketAllocationDetail { get; set; }
        public IEnumerable<CorporateTransactionDetail> CorporateTransactionDetail { get; set; }
        public IEnumerable<Sponsor> Sponsor { get; set; }
        public IEnumerable<FIL.Contracts.Models.Transaction> Transaction { get; set; }
        public IEnumerable<TransactionDetail> TransactionDetail { get; set; }
        public IEnumerable<ReportingColumn> ReportColumns { get; set; }
    }
}
