using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice.SeatLayout;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice.SeatLayout
{
    public class BlockCustomerSeatCommandHandler : BaseCommandHandler<BlockCustomerSeatCommand>
    {
        public IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;

        public BlockCustomerSeatCommandHandler(IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository, IMediator mediator)
            : base(mediator)
        {
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
        }

        protected override async Task Handle(BlockCustomerSeatCommand command)
        {
            SeatStatus BlockByCustomer = SeatStatus.BlockedByCustomer;
            List<MatchLayoutSectionSeat> matchLayoutSectionSeats = _matchLayoutSectionSeatRepository.GetByIds(command.MatchLayoutSectionSeatsIds.Select(s => s)).ToList();
            foreach (var matchLayoutSectionSeat in matchLayoutSectionSeats)
            {
                if (!command.IsBlock)
                {
                    BlockByCustomer = SeatStatus.UnSold;
                }

                matchLayoutSectionSeat.SeatStatusId = BlockByCustomer;
                matchLayoutSectionSeat.UpdatedUtc = DateTime.UtcNow;
                _matchLayoutSectionSeatRepository.Save(matchLayoutSectionSeat);
            }
        }
    }
}