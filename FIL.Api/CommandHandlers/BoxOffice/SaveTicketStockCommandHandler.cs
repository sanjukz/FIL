using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class SaveTicketStockCommandHandler : BaseCommandHandlerWithResult<TicketStockCommand, TicketStockCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IBoUserVenueRepository _boUserVenueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITicketStockDetailRepository _ticketStockDetailRepository;

        public SaveTicketStockCommandHandler(IUserRepository userRepository, ITicketStockDetailRepository ticketStockDetailRepository, IEventAttributeRepository eventAttributeRepository, IBoUserVenueRepository boUserVenueRepository, IEventDetailRepository eventDetailRepository, IMediator mediator) : base(mediator)
        {
            _userRepository = userRepository;
            _ticketStockDetailRepository = ticketStockDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _boUserVenueRepository = boUserVenueRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        protected override Task<ICommandResult> Handle(TicketStockCommand command)
        {
            // TODO: Refactor this logic into a provider
            var userId = _userRepository.GetByAltId(command.UserAltId).Id;
            var eventId = _boUserVenueRepository.GetEventByUserId(userId).EventId;
            var userVenues = _boUserVenueRepository.GetByUserIdAndEventId(eventId, userId).Select(s => s.VenueId).Distinct();
            var eventDetails = _eventDetailRepository.GetByEventIdAndVenueIds(eventId, userVenues);
            var eventAttributeList = _eventAttributeRepository.GetByEventDetailIds(eventDetails.Select(s => s.Id).Distinct()).ToList();
            //int timeZone = eventAttributeList.Count > 0 ? Convert.ToInt16(eventAttributeList[0].TimeZone) : 0;
            _ticketStockDetailRepository.Save(new TicketStockDetail
            {
                UserId = userId,
                TicketStockStartSrNo = command.TicketStockStartSrNo,
                TicketStockEndSrNo = command.TicketStockEndSrNo,
                IsEnabled = true,
                CreatedUtc = DateTime.UtcNow.AddMinutes(0),
                CreatedBy = command.UserAltId
            });
            TicketStockCommandResult ticketStockCommandResult = new TicketStockCommandResult
            {
                Success = true,
                Message = "Ticket stock submitted successfully"
            };
            return Task.FromResult<ICommandResult>(ticketStockCommandResult);
        }
    }
}