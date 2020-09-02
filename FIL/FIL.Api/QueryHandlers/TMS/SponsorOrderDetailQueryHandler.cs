using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResults.TMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS
{
    public class SponsorOrderDetailQueryHandler : IQueryHandler<SponsorOrderDetailQuery, SponsorOrderDetailQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ICorporateOrderRequestRepository _corporateOrderRequestRepository;

        public SponsorOrderDetailQueryHandler(IEventRepository eventRepository,
            IVenueRepository venueRepository,
            IEventDetailRepository eventDetailRepository,
            ICorporateOrderRequestRepository corporateOrderRequestRepository)
        {
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _eventDetailRepository = eventDetailRepository;
            _corporateOrderRequestRepository = corporateOrderRequestRepository;
        }

        public SponsorOrderDetailQueryResult Handle(SponsorOrderDetailQuery query)
        {
            var events = _eventRepository.GetByAltId(query.EventAltId);
            var venues = _venueRepository.GetByAltId(query.VenueAltId);
            List<EventDetail> eventDetails = new List<EventDetail>();
            if (query.EventDetailAltId != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                eventDetails = AutoMapper.Mapper.Map<List<EventDetail>>(_eventDetailRepository.GetEventDetailByAltId(query.EventDetailAltId)).ToList();
            }
            else
            {
                eventDetails = AutoMapper.Mapper.Map<List<EventDetail>>(_eventDetailRepository.GetByEventIdAndVenueId(events.Id, venues.Id));
            }
            var sponsorOrderDetails = _corporateOrderRequestRepository.GetSponsorOrderDetails(eventDetails.Select(s => s.Id).Distinct()).ToList();
            return new SponsorOrderDetailQueryResult
            {
                SponsorOrderDetails = sponsorOrderDetails,
            };
        }
    }
}