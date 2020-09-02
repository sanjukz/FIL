using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.TransactionReport
{
    public class GetAllReportPlacesResponseModel
    {
        public List<FIL.Contracts.Models.TransactionReport.FAPAllPlacesResponseModel> Places { get; set; }
    }
}
