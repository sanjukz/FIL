using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.TransactionReport;
using System;

namespace FIL.Contracts.Queries.TransactionReport
{
    public class TransactionReportQuery : IQuery<TransactionReportQueryResult>
    {
        public string EventAltId { get; set; }
        public string CurrencyTypes { get; set; }
        public Guid UserAltId { get; set; }
        public string EventDetailId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}