using FIL.Api.Repositories;
using FIL.Contracts.Commands.SeatLayout;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.SeatLayout
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
            MatchLayoutSectionSeat matchLayoutSectionSeat = _matchLayoutSectionSeatRepository.Get(command.MatchLayoutSectionSeatsId);
            if (!command.IsBlock)
            {
                BlockByCustomer = SeatStatus.UnSold;
            }
            if (matchLayoutSectionSeat != null)
            {
                matchLayoutSectionSeat.SeatStatusId = BlockByCustomer;
                matchLayoutSectionSeat.UpdatedUtc = DateTime.UtcNow;
                _matchLayoutSectionSeatRepository.Save(matchLayoutSectionSeat);
            }
        }
    }
}