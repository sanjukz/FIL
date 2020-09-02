using FIL.Api.Repositories;
using FIL.Contracts.Commands.UserCreation;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.UserCreation
{
    public class EventAssignCommandHandler : BaseCommandHandler<EventAssignCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _event;
        private readonly IEventDetailRepository _eventDetail;
        private readonly IVenueRepository _venueRepository;
        private readonly IBoUserVenueRepository _boUserVenueRepository;
        private readonly IMediator _mediator;

        public EventAssignCommandHandler(
            IUserRepository userRepository,
            IEventRepository events,
            IEventDetailRepository eventDetail,
            IVenueRepository venueRepository,
            IBoUserVenueRepository boUserVenueRepository,
            IMediator mediator)
            : base(mediator)
        {
            _userRepository = userRepository;
            _event = events;
            _eventDetail = eventDetail;
            _venueRepository = venueRepository;
            _boUserVenueRepository = boUserVenueRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(EventAssignCommand command)
        {
            var user = _userRepository.GetByAltId(new Guid(command.UserAltId));

            if (user != null)
            {
                foreach (var eventId in command.EventIds)
                {
                    var venues = _eventDetail.GetSubEventByEventId(eventId).Select(e => e.VenueId).Distinct().ToList();

                    foreach (var venueId in venues)
                    {
                        var BoUserVenue = new FIL.Contracts.DataModels.BoUserVenue
                        {
                            AltId = Guid.NewGuid(),
                            EventId = eventId,
                            VenueId = venueId,
                            UserId = user.Id,
                            CreatedBy = command.ModifiedBy,
                            CreatedUtc = DateTime.UtcNow,
                            IsEnabled = true
                        };
                        _boUserVenueRepository.Save(BoUserVenue);
                    }
                }
            }
        }
    }
}