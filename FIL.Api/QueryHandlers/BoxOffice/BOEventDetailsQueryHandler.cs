using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.Events;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class EventDetailsQueryHandler : IQueryHandler<BOEventDetailQuery, EventDetailQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;

        public EventDetailsQueryHandler(IEventRepository eventRepository, IEventDetailRepository eventDetailRepository, IVenueRepository venueRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _eventRepository = eventRepository;
        }

        public EventDetailQueryResult Handle(BOEventDetailQuery query)
        {
            var venueId = _venueRepository.GetByAltId(query.VenueAltId).Id;
            var eventid = _eventRepository.GetByAltId(query.EventAltId).Id;
            var eventdetails = _eventDetailRepository.GetBOByEventIdAndVenueId(eventid, venueId);
            return new EventDetailQueryResult
            {
                EventDetails = AutoMapper.Mapper.Map<List<EventDetail>>(eventdetails)
            };
        }
    }
}