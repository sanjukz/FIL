using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class ReportSubEventQueryHandler : IQueryHandler<ReportSubEventsQuery, ReportSubEventsQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoUserVenueRepository _boUserVenueRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;

        public ReportSubEventQueryHandler(IVenueRepository venueRepository, IEventRepository eventRepository, IEventDetailRepository eventDetailRepository, IUserRepository userRepository, IBoUserVenueRepository boUserVenueRepository)
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _userRepository = userRepository;
            _boUserVenueRepository = boUserVenueRepository;
        }

        public ReportSubEventsQueryResult Handle(ReportSubEventsQuery query)
        {
            var eventid = _eventRepository.GetByAltId(query.EventAltId).Id;
            var userId = _userRepository.GetByAltId(query.UserAltId).Id;
            var uservenues = _boUserVenueRepository.GetByUserIdAndEventId(eventid, userId).Select(s => s.VenueId);
            var eventDetails = _eventDetailRepository.GetAllByEventIdAndVenueIdsBo(eventid, uservenues);
            return new ReportSubEventsQueryResult
            {
                SubEvents = AutoMapper.Mapper.Map<IEnumerable<EventDetail>>(eventDetails)
            };
        }
    }
}