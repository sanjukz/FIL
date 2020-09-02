using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResult.TMS;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.TMS
{
    public class EventDetailsQueryHandler : IQueryHandler<EventDetailQuery, EventDetailQueryResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;

        public EventDetailsQueryHandler(IEventDetailRepository eventDetailRepository,
            IEventRepository eventRepository,
            IVenueRepository venueRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
        }

        public EventDetailQueryResult Handle(EventDetailQuery query)
        {
            var eventId = _eventRepository.GetByAltId(query.EventAltId).Id;
            var venueId = _venueRepository.GetByAltId(query.VenueAltId).Id;
            var eventdetails = _eventDetailRepository.GetEventDetailForTMS(eventId, venueId);
            return new EventDetailQueryResult
            {
                eventDetails = AutoMapper.Mapper.Map<List<EventDetail>>(eventdetails)
            };
        }
    }
}