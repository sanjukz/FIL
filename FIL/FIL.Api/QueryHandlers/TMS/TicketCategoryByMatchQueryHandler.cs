using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResult.TMS;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS
{
    public class TicketCategoryByMatchQueryHandler : IQueryHandler<TicketCategoryByMatchQuery, TicketCategoryByMatchQueryResult>
    {
        private readonly IVenueRepository _venuetRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;

        public TicketCategoryByMatchQueryHandler(
            IVenueRepository venuetRepository,
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository)
        {
            _venuetRepository = venuetRepository;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
        }

        public TicketCategoryByMatchQueryResult Handle(TicketCategoryByMatchQuery query)
        {
            List<Contracts.Models.TicketCategory> ticketCategories = new List<Contracts.Models.TicketCategory>();
            Event events = new Event();
            Venue venue = new Venue();
            EventDetail eventDetail = new EventDetail();
            List<EventDetail> eventDetailList = new List<EventDetail>();
            List<EventTicketDetail> eventTicketDetails = new List<EventTicketDetail>();

            if (query.AllocationType == AllocationType.Match)
            {
                eventDetail = _eventDetailRepository.GetByAltId(query.EventDetailAltId);
                eventTicketDetails = AutoMapper.Mapper.Map<List<EventTicketDetail>>(_eventTicketDetailRepository.GetByEventDetailId(eventDetail.Id));
                ticketCategories = AutoMapper.Mapper.Map<List<Contracts.Models.TicketCategory>>(_ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetails.Select(s => s.TicketCategoryId)));
            }
            else if (query.AllocationType == AllocationType.Venue)
            {
                venue = _venuetRepository.GetByAltId(query.VenueAltId);
                events = _eventRepository.GetByAltId(query.EventAltId);
                eventDetailList = AutoMapper.Mapper.Map<List<EventDetail>>(_eventDetailRepository.GetEventDetailForTMS(events.Id, venue.Id));
                eventTicketDetails = AutoMapper.Mapper.Map<List<EventTicketDetail>>(_eventTicketDetailRepository.GetAllByEventDetailIds(eventDetailList.Select(s => s.Id)));
                ticketCategories = AutoMapper.Mapper.Map<List<Contracts.Models.TicketCategory>>(_ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetails.Select(s => s.TicketCategoryId).Distinct()));
            }
            else if (query.AllocationType == AllocationType.Sponsor)
            {
                /*TODO*/
            }
            return new TicketCategoryByMatchQueryResult
            {
                ticketCategories = ticketCategories
            };
        }
    }
}