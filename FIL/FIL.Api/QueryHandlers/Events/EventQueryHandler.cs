using FIL.Api.Repositories;
using FIL.Contracts.Queries.Events;
using FIL.Contracts.QueryResults.Events;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Events
{
    public class EventQueryHandler : IQueryHandler<EventQuery, EventQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoUserVenueRepository _boUserVenueRepository;
        private readonly IEventRepository _eventRepository;

        public EventQueryHandler(IEventRepository eventRepository, IBoUserVenueRepository boUserVenueRepository, IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _boUserVenueRepository = boUserVenueRepository;
            _userRepository = userRepository;
        }

        public EventQueryResult Handle(EventQuery query)
        {
            var userId = _userRepository.GetByAltId(query.AltId).Id;
            var userEvents = _boUserVenueRepository.GetEventsByUserId(userId).Where(w => w.IsEnabled).Distinct();
            var eventResult = _eventRepository.GetBOEventByEventIds(userEvents.Select(s => s.EventId)).Distinct();
            return new EventQueryResult
            {
                Events = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Event>>(eventResult)
            };
        }
    }
}