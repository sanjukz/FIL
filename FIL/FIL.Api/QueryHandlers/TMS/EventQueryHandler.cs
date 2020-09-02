using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResult.TMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS
{
    public class EventQueryHandler : IQueryHandler<EventQuery, EventQueryResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;

        public EventQueryHandler(IEventDetailRepository eventDetailRepository, IEventRepository eventRepository, IVenueRepository venueRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
        }

        public EventQueryResult Handle(EventQuery query)
        {
            var venueId = _venueRepository.GetByAltId(query.VenueAltId).Id;
            var eventdetails = _eventDetailRepository.GetByVenueId(venueId).Where(w => w.CreatedUtc >= DateTime.UtcNow.AddYears(-1));
            var events = _eventRepository.GetTMSEventByEventIds(eventdetails.Select(s => s.EventId).Distinct());
            return new EventQueryResult
            {
                events = AutoMapper.Mapper.Map<List<Event>>(events)
            };
        }
    }
}