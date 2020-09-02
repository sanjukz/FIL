using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Reporting;
using System;

namespace FIL.Contracts.Queries.Reporting
{
    public class FailedTransactionReportQuery : IQuery<FailedTransactionReportQueryResult>
    {
        public Guid UserAltId { get; set; }
        public Guid EventAltId { get; set; }
        public long EventDetailId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
    }
}