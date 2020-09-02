using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Web.Kitms.Feel.ViewModels.Reporting
{
    public class TransactionReportResponseViewModel
    {
        public List<FIL.Contracts.Commands.Transaction.TransactionReport> TransactionReport { get; set; }
        public List<FIL.Contracts.Models.ReportingColumn> ReportColumns { get; set; }
    }
}
