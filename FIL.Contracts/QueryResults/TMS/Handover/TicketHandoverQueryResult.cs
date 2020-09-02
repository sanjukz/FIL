using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TMS.Handover
{
    public class TicketHandoverQueryResult
    {
        public IEnumerable<FIL.Contracts.Models.TMS.TicketHandoverDetail> ticketHandoverDetails { get; set; }
        public IEnumerable<FIL.Contracts.Models.TMS.SponsorTicketDetail> sponsorTicketDetails { get; set; }
    }
}