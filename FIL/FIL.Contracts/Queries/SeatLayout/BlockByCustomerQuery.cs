using FIL.Contracts.Interfaces.Queries;

namespace FIL.Contracts.Queries.SeatLayout
{
    public class BlockByCustomerQuery : IQuery<BlockByCustomerQueryResult>
    {
        public long MatchLayoutSectionSeatsId { get; set; }
    }
}