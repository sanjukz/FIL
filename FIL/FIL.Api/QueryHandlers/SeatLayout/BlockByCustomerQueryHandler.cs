using FIL.Api.Repositories;
using FIL.Contracts.Queries.SeatLayout;

namespace FIL.Api.QueryHandlers.SeatLayout
{
    public class BlockByCustomerQueryHandler : IQueryHandler<BlockByCustomerQuery, BlockByCustomerQueryResult>
    {
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;

        public BlockByCustomerQueryHandler(IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository)
        {
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
        }

        public BlockByCustomerQueryResult Handle(BlockByCustomerQuery query)
        {
            Contracts.DataModels.MatchLayoutSectionSeat matchLayoutSectionSeat = _matchLayoutSectionSeatRepository.Get(query.MatchLayoutSectionSeatsId);
            if (matchLayoutSectionSeat.SeatStatusId == Contracts.Enums.SeatStatus.UnSold)
            {
                return new BlockByCustomerQueryResult
                {
                    isAlreadyBlocked = true
                };
            }
            else
            {
                return new BlockByCustomerQueryResult
                {
                    isAlreadyBlocked = false
                };
            }
        }
    }
}