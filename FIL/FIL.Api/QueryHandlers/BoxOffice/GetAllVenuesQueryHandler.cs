using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Events
{
    public class VenuesQueryHandler : IQueryHandler<GetAllVenuesQuery, GetAllVenueQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;

        public VenuesQueryHandler(IEventRepository eventRepository, IEventDetailRepository eventDetailRepository, IVenueRepository venueRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
        }

        public GetAllVenueQueryResult Handle(GetAllVenuesQuery query)
        {
            var eventid = _eventRepository.GetByAltId(query.EventAltID).Id;
            var eventDetails = _eventDetailRepository.GetAllByEventId(eventid);
            var venues = _venueRepository.GetByVenueIds(eventDetails.Select(s => s.VenueId).Distinct());
            return new GetAllVenueQueryResult
            {
                Venues = AutoMapper.Mapper.Map<List<Venue>>(venues)
            };
        }
    }
}