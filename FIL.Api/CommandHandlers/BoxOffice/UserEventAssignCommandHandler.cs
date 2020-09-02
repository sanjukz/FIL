using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BoxOffice
{
    public class UserEventAssignCommandHandler : BaseCommandHandler<UserEventAssignCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IBoUserVenueRepository _boUserVenueRepository;
        private readonly IEventsUserMappingRepository _eventsUserMappingRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEventDetailRepository _eventDetailRepository;

        public UserEventAssignCommandHandler(IUserRepository userRepository, IVenueRepository venueRepository, IBoUserVenueRepository boUserVenueRepository,
            IEventRepository eventRepository,
            IEventsUserMappingRepository eventsUserMappingRepository,
            IRoleRepository roleRepository,
            IEventDetailRepository eventDetailRepository,
        IMediator mediator) : base(mediator)
        {
            _userRepository = userRepository;
            _venueRepository = venueRepository;
            _boUserVenueRepository = boUserVenueRepository;
            _eventRepository = eventRepository;
            _eventsUserMappingRepository = eventsUserMappingRepository;
            _roleRepository = roleRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        protected override async Task Handle(UserEventAssignCommand command)
        {
            var eventId = _eventRepository.GetByAltId(command.EventAltId).Id;
            var venueId = _venueRepository.GetByAltId(command.VenueAltId).Id;
            var eventDetails = _eventDetailRepository.GetByEventIdAndVenueId(eventId, venueId);
            var users = _userRepository.GetByAltId(command.UserAltId);
            var roles = _roleRepository.Get(users.RolesId);

            List<EventsUserMapping> eventsUserMappings = new List<EventsUserMapping>();
            if (roles.ModuleId == FIL.Contracts.Enums.Modules.Boxoffice)
            {
                var bouserVenue = new BoUserVenue
                {
                    AltId = Guid.NewGuid(),
                    UserId = users.Id,
                    EventId = eventId,
                    VenueId = venueId,
                    IsEnabled = true,
                    ModifiedBy = command.ModifiedBy
                };
                _boUserVenueRepository.Save(bouserVenue);
            }
            else
            {
                foreach (var item in eventDetails)
                {
                    var eventsUserMapping = new EventsUserMapping
                    {
                        UserId = users.Id,
                        EventId = item.EventId,
                        EventDetailId = item.Id,
                        IsEnabled = true,
                        ModifiedBy = command.ModifiedBy
                    };
                    _eventsUserMappingRepository.Save(eventsUserMapping);
                }
            }
        }
    }
}