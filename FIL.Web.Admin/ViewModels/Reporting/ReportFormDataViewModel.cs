using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Reporting
{
    public class ReportFormDataViewModel
    {
        public Guid UserAltId { get; set; }
        public Guid EventAltId { get; set; }
        public long EventDetailId { get; set; }
        public Guid EventDetailAltId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int PageNumber { get; set; }
        public int NoRecordsPerPage { get; set; }
    }
}
