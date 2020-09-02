using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.TournamentLayout;
using FIL.Contracts.QueryResults.TournamentLayout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TournamentLayouts
{
    public class TournamentLayoutEventVenueQueryHandler : IQueryHandler<TournamentEventVenueQuery, TournamentLayoutEventVenueQueryResult>
    {
        private readonly IVenueRepository _venueRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;

        public TournamentLayoutEventVenueQueryHandler(IVenueRepository venueRepository, IEventRepository eventRepository, IEventDetailRepository eventDetailRepository)
        {
            _venueRepository = venueRepository;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        public TournamentLayoutEventVenueQueryResult Handle(TournamentEventVenueQuery query)
        {
            var eventid = _eventRepository.GetByAltId(query.EventAltId).Id;
            var eventDetails = _eventDetailRepository.GetSubeventByEventId(Convert.ToInt32(eventid));
            var venues = _venueRepository.GetByVenueIds(eventDetails.Select(s => s.VenueId)).Distinct();

            return new TournamentLayoutEventVenueQueryResult
            {
                Venues = AutoMapper.Mapper.Map<List<Venue>>(venues)
            };
        }
    }
}