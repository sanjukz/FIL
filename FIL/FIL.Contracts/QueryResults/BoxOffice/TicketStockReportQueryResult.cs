using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class TicketStockReportQueryResult
    {
        public List<TicketStockDetail> TicketStockDetails { get; set; }
        public List<FIL.Contracts.Models.User> Users { get; set; }
    }
}