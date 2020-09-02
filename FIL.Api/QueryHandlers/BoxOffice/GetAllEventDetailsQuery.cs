using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.Events;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.Events
{
    public class GetAllEventDetailsQueryHandler : IQueryHandler<GetAllEventDetailsQuery, EventDetailQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;

        public GetAllEventDetailsQueryHandler(IEventRepository eventRepository, IEventDetailRepository eventDetailRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
        }

        public EventDetailQueryResult Handle(GetAllEventDetailsQuery query)
        {
            var eventid = _eventRepository.GetByAltId(query.EventAltId).Id;
            var eventdetails = _eventDetailRepository.GetSubEventByEventId(eventid);
            return new EventDetailQueryResult
            {
                EventDetails = AutoMapper.Mapper.Map<List<EventDetail>>(eventdetails)
            };
        }
    }
}