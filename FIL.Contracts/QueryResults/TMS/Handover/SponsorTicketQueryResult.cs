using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TMS.Handover
{
    public class SponsorTicketQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.TMS.SponsorTicketDetail> sponsorTicketDetails { get; set; }
    }
}