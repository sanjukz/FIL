using System;
using FIL.Contracts.Enums;

namespace FIL.Web.Kitms.Feel.ViewModels.Reporting
{
    public class FailedTransactionReportFormDataViewModel
    {
        public Guid EventAltId { get; set; }
        public long EventDetailId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public PaymentGateway paymentGateway { get; set; }
    }
}
