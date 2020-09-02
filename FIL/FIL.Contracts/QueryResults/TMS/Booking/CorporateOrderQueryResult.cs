using FIL.Contracts.Models.TMS;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TMS.Booking
{
    public class CorporateOrderQueryResult
    {
        public List<CorporateTicketAllocationDetail> corporateTicketAllocationDetails { get; set; }
    }
}