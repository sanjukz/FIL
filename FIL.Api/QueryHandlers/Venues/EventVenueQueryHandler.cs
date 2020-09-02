using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Venue;
using FIL.Contracts.QueryResults;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Venues
{
    public class EventVenueQueryHandler : IQueryHandler<EventVenueQuery, EventVenueQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoUserVenueRepository _boUserVenueRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;

        public EventVenueQueryHandler(IVenueRepository venueRepository, IEventRepository eventRepository, IEventDetailRepository eventDetailRepository, IUserRepository userRepository, IBoUserVenueRepository boUserVenueRepository)
        {
            _venueRepository = venueRepository;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _userRepository = userRepository;
            _boUserVenueRepository = boUserVenueRepository;
        }

        public EventVenueQueryResult Handle(EventVenueQuery query)
        {
            var eventid = _eventRepository.GetByAltId(query.EventAltId).Id;
            var userId = _userRepository.GetByAltId(query.AltId).Id;
            var uservenues = _boUserVenueRepository.GetByUserIdAndEventId(eventid, userId);
            var eventDetails = _eventDetailRepository.GetByEventIdAndVenueIds(eventid, uservenues.Select(s => s.VenueId));
            var venues = _venueRepository.GetByVenueIds(eventDetails.Select(s => s.VenueId)).Distinct();
            return new EventVenueQueryResult
            {
                Venues = AutoMapper.Mapper.Map<List<Venue>>(venues)
            };
        }
    }
}