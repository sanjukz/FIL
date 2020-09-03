using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Reporting
{
    public class ReportSubEventsResponseDataViewModel
    {
        public IEnumerable<FIL.Contracts.Models.EventDetail> SubEvents { get; set; }
        public IEnumerable<FIL.Contracts.DataModels.CurrencyType> CurrencyTypes { get; set; }
        
    }
}
