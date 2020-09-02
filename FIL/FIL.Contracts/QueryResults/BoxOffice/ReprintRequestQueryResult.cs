using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class ReprintRequestQueryResult
    {
        public IEnumerable<MatchSeatTicketDetail> MatchSeatTicketDetail { get; set; }
        public List<ReprintRequestContainer> ReprintRequestContainers { get; set; }
    }
}