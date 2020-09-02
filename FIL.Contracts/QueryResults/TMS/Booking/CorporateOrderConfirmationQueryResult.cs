using FIL.Contracts.Models.TMS;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TMS.Booking
{
    public class CorporateOrderConfirmationQueryResult
    {
        public List<CorporateOrderDetails> corporateOrderDetails { get; set; }
    }
}