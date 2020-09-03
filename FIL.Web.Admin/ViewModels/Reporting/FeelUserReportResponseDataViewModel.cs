using System;
using System.Collections.Generic;
using FIL.Contracts.Enums;

namespace FIL.Web.Admin.ViewModels.Reporting
{
    public class FeelUserReportResponseDataViewModel
    {
        public List<FIL.Contracts.Models.Report.FeelUserReport> FeelUsers { get; set; }
    }
}
