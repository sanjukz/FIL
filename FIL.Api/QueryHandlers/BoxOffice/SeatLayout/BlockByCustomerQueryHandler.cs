using FIL.Api.Repositories;
using FIL.Contracts.Queries.BoxOffice.SeatLayout;
using FIL.Contracts.QueryResults.Boxoffice.SeatLayout;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice.SeatLayout
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
            List<long> alreadyBookedSeats = new List<long>();
            List<long> availabelSeats = new List<long>();

            List<Contracts.DataModels.MatchLayoutSectionSeat> matchLayoutSectionSeats = _matchLayoutSectionSeatRepository.GetByIds(query.MatchLayoutSectionSeatsIds.Select(s => s)).ToList();
            foreach (var matchLayoutSectionSeat in matchLayoutSectionSeats)
            {
                if (matchLayoutSectionSeat.SeatStatusId == Contracts.Enums.SeatStatus.UnSold)
                {
                    availabelSeats.Add(matchLayoutSectionSeat.Id);
                }
                else
                {
                    alreadyBookedSeats.Add(matchLayoutSectionSeat.Id);
                }
            }
            return new BlockByCustomerQueryResult
            {
                AlreadyBookedSeats = alreadyBookedSeats,
                AvailabelSeats = availabelSeats
            };
        }
    }
}