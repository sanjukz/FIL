using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Boxoffice.SeatLayout;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.BoxOffice.SeatLayout
{
    public class BlockByCustomerQuery : IQuery<BlockByCustomerQueryResult>
    {
        public List<long> MatchLayoutSectionSeatsIds { get; set; }
    }
}