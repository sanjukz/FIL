using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.TMS;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResult.TMS;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.TMS
{
    public class StandDetailByMatchQueryHandler : IQueryHandler<StandDetailByMatchQuery, StandDetailByMatchQueryResult>
    {
        private readonly IVenueRepository _venuetRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IStandDetailRepository _standDetailRepository;

        public StandDetailByMatchQueryHandler(
         IVenueRepository venuetRepository,
         IEventRepository eventRepository,
         IEventDetailRepository eventDetailRepository,
         IStandDetailRepository standDetailRepository)
        {
            _venuetRepository = venuetRepository;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _standDetailRepository = standDetailRepository;
        }

        public StandDetailByMatchQueryResult Handle(StandDetailByMatchQuery query)
        {
            List<StandDetail> standDetails = new List<StandDetail>();
            if (query.AllocationType == AllocationType.Match)
            {
                var eventDetails = _eventDetailRepository.GetByAltId((Guid)query.EventDetailAltId);
                standDetails = _standDetailRepository.GetStandData(eventDetails.Id, (long)query.TicketCategoryId);
            }
            else if (query.AllocationType == AllocationType.Venue)
            {
                var venueId = _venuetRepository.GetByAltId((Guid)query.VenueAltId).Id;
                var eventId = _eventRepository.GetByAltId((Guid)query.EventAltId).Id;
                standDetails = _standDetailRepository.GetStandDataByVenue(eventId, venueId, (long)query.TicketCategoryId);
            }
            else if (query.AllocationType == AllocationType.Sponsor)
            {
                /* TODO: */
            }
            return new StandDetailByMatchQueryResult
            {
                standDetails = standDetails
            };
        }
    }
}