using FIL.Contracts.Models.TMS;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TMS
{
    public class BarcodeDetailQueryResult
    {
        public List<TicketDetailModel> TicketDetailModel { get; set; }
    }
}