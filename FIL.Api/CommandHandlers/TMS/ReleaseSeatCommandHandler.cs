using FIL.Api.Repositories;
using FIL.Contracts.Commands.TMS;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using MediatR;
using System;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TMS
{
    public class ReleaseSeatCommandHandler : BaseCommandHandler<ReleaseSeatCommand>
    {
        public IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        public IEventTicketDetailRepository _eventTicketDetailRepository;
        public IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly Logging.ILogger _logger;

        public ReleaseSeatCommandHandler(IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            Logging.ILogger logger,
            IMediator mediator)
            : base(mediator)
        {
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _logger = logger;
        }

        protected override async Task Handle(ReleaseSeatCommand command)
        {
            try
            {
                foreach (var item in command.SeatDetails)
                {
                    var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailId(item.EventTicketDetailId);
                    MatchLayoutSectionSeat matchLayoutSectionSeat = _matchLayoutSectionSeatRepository.GetByIdandEventTicketDetailId(item.MatchLayoutSectionSeatId, item.EventTicketDetailId);
                    if (matchLayoutSectionSeat.SeatTypeId == SeatType.Blocked)
                    {
                        matchLayoutSectionSeat.SeatTypeId = SeatType.Available;
                        matchLayoutSectionSeat.UpdatedBy = command.ModifiedBy;
                        _matchLayoutSectionSeatRepository.Save(matchLayoutSectionSeat);
                    }
                    eventTicketAttributes.RemainingTicketForSale += 1;
                    eventTicketAttributes.UpdatedBy = command.ModifiedBy;
                    _eventTicketAttributeRepository.Save(eventTicketAttributes);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
        }
    }
}