using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class SaveClosingStockCommandHandler : BaseCommandHandlerWithResult<ClosingStockCommand, ClosingStockCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoClosingDetailRepository _boClosingDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IBoUserVenueRepository _boUserVenueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;

        public SaveClosingStockCommandHandler(IUserRepository userRepository, IBoClosingDetailRepository boClosingDetailRepository, IEventAttributeRepository eventAttributeRepository, IBoUserVenueRepository boUserVenueRepository, IEventDetailRepository eventDetailRepository, IMediator mediator) : base(mediator)
        {
            _userRepository = userRepository;
            _boClosingDetailRepository = boClosingDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _boUserVenueRepository = boUserVenueRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        protected override Task<ICommandResult> Handle(ClosingStockCommand command)
        {
            var userId = _userRepository.GetByAltId(command.UserAltId).Id;
            var eventId = _boUserVenueRepository.GetEventByUserId(userId).EventId;
            var userVenues = _boUserVenueRepository.GetByUserIdAndEventId(eventId, userId).Select(s => s.VenueId).Distinct();
            var eventDetails = _eventDetailRepository.GetByEventIdAndVenueIds(eventId, userVenues);
            var eventAttributeList = _eventAttributeRepository.GetByEventDetailIds(eventDetails.Select(s => s.Id).Distinct()).ToList();
            int timeZone = eventAttributeList.Count > 0 ? Convert.ToInt16(eventAttributeList[0].TimeZone) : 0;
            var boClosingDetail = AutoMapper.Mapper.Map<BoClosingDetail>(_boClosingDetailRepository.GetByUserIdDate(userId, DateTime.UtcNow.AddMinutes(timeZone).Date));
            if (boClosingDetail == null)
            {
                _boClosingDetailRepository.Save(new BoClosingDetail
                {
                    UserId = userId,
                    TicketStockStartSrNo = command.TicketStockStartSrNo,
                    WasteTickets = command.WasteTickets,
                    CashAmount = command.CashAmount,
                    CardAmount = command.CardAmount,
                    IsEnabled = true,
                    CreatedUtc = DateTime.UtcNow.AddMinutes(timeZone),
                    CreatedBy = command.UserAltId
                });
                ClosingStockCommandResult closingStockCommandResult = new ClosingStockCommandResult
                {
                    Success = true,
                    Message = "Details submitted successfully"
                };
                return Task.FromResult<ICommandResult>(closingStockCommandResult);
            }
            else
            {
                ClosingStockCommandResult closingStockCommandResult = new ClosingStockCommandResult
                {
                    Success = false,
                    Message = "Closing details already submitted, user can submit details only once in a day"
                };
                return Task.FromResult<ICommandResult>(closingStockCommandResult);
            }
        }
    }
}