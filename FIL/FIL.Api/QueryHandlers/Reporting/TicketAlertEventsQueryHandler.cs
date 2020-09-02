using FIL.Api.Repositories;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Reporting
{
    public class TicketAlertEventsQueryHandler : IQueryHandler<TicketAlertEventsQuery, TicketAlertEventsQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventsUserMappingRepository _eventsUserMappingRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITicketAlertEventMappingRepository _ticketAlertEventMappingRepository;
        private readonly IEventRepository _eventRepository;

        public TicketAlertEventsQueryHandler(
            IUserRepository userRepository,
            IEventsUserMappingRepository eventsUserMappingRepository,
            ITicketAlertEventMappingRepository ticketAlertEventMappingRepository,
            IEventDetailRepository eventDetailRepository,
            IEventRepository eventRepository
        )
        {
            _userRepository = userRepository;
            _eventsUserMappingRepository = eventsUserMappingRepository;
            _ticketAlertEventMappingRepository = ticketAlertEventMappingRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
        }

        public TicketAlertEventsQueryResult Handle(TicketAlertEventsQuery query)
        {
            var ticketAlertEvents = _ticketAlertEventMappingRepository.GetAll();

            var events = _eventRepository.GetByAllEventIds(ticketAlertEvents.Select(s => s.EventId).Distinct());

            return new TicketAlertEventsQueryResult
            {
                Events = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.Event>>(events)
            };
        }
    }
}