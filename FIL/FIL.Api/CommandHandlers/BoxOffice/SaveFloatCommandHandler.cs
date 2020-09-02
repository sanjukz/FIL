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
    public class SaveFloatCommandHandler : BaseCommandHandlerWithResult<FloatCommand, FloatCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IBoUserVenueRepository _boUserVenueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IFloatDetailRepository _floatDetailRepository;

        public SaveFloatCommandHandler(IUserRepository userRepository, IFloatDetailRepository floatDetailRepository, IEventAttributeRepository eventAttributeRepository, IBoUserVenueRepository boUserVenueRepository, IEventDetailRepository eventDetailRepository, IMediator mediator) : base(mediator)
        {
            _userRepository = userRepository;
            _floatDetailRepository = floatDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _boUserVenueRepository = boUserVenueRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        protected override Task<ICommandResult> Handle(FloatCommand command)
        {
            var userId = _userRepository.GetByAltId(command.UserAltId).Id;
            var eventId = _boUserVenueRepository.GetEventByUserId(userId).EventId;
            var userVenues = _boUserVenueRepository.GetByUserIdAndEventId(eventId, userId).Select(s => s.VenueId).Distinct();
            var eventDetails = _eventDetailRepository.GetByEventIdAndVenueIds(eventId, userVenues);
            var eventAttributeList = _eventAttributeRepository.GetByEventDetailIds(eventDetails.Select(s => s.Id).Distinct()).ToList();
            int timeZone = eventAttributeList.Any() ? Convert.ToInt16(eventAttributeList[0].TimeZone) : 0;
            //var floatDetail = AutoMapper.Mapper.Map<FloatDetail>(_floatDetailRepository.GetByUserIdDate(userId, DateTime.UtcNow.AddMinutes(timeZone).Date));
            var floatDetail = AutoMapper.Mapper.Map<FloatDetail>(_floatDetailRepository.GetByUserIdDate(userId, DateTime.UtcNow.AddMinutes(0).Date));
            if (floatDetail == null)
            {
                _floatDetailRepository.Save(new FloatDetail
                {
                    UserId = userId,
                    CashInHand = command.CashInHand,
                    CashInHandLocal = command.CashInHandLocal,
                    IsEnabled = true,
                    //CreatedUtc = DateTime.UtcNow.AddMinutes(timeZone),
                    // ModifiedBy = command.UserAltId
                });
                var floatCommandResult = new FloatCommandResult
                {
                    Success = true,
                    Message = "Float amount for today is – USD: " + command.CashInHand + " – Local: " +
                              command.CashInHandLocal + ""
                };
                return Task.FromResult<ICommandResult>(floatCommandResult);
            }
            else
            {
                var floatCommandResult = new FloatCommandResult
                {
                    Success = false,
                    Message = "You can enter details only once in a day. Float amount for today is – USD: " + floatDetail.CashInHand + " – Local: " + floatDetail.CashInHandLocal.ToString() + ""
                };
                return Task.FromResult<ICommandResult>(floatCommandResult);
            }
        }
    }
}