using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class GetTicketInfoQueryResult
    {
        public IEnumerable<MatchSeatTicketInfo> matchSeatTicketInfo { get; set; }
        public IEnumerable<MatchTeamInfo> matchTeamInfo { get; set; }
    }
}