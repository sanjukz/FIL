using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TransactionReport
{
    public class ReportEventsQueryHandler : IQueryHandler<ReportEventsQuery, ReportEventsQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventsUserMappingRepository _eventsUserMappingRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;

        public ReportEventsQueryHandler(
            IUserRepository userRepository,
            IEventsUserMappingRepository eventsUserMappingRepository,
            IEventDetailRepository eventDetailRepository,
            IEventRepository eventRepository
        )
        {
            _userRepository = userRepository;
            _eventsUserMappingRepository = eventsUserMappingRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
        }

        public ReportEventsQueryResult Handle(ReportEventsQuery query)
        {
            List<FIL.Contracts.DataModels.EventsUserMapping> assignedEvents = new List<FIL.Contracts.DataModels.EventsUserMapping>();
            List<FIL.Contracts.DataModels.Event> events = new List<FIL.Contracts.DataModels.Event>();
            List<Event> eventList = new List<Event>();
            var userDetails = _userRepository.GetByAltId(query.AltId);
            if (userDetails.RolesId == 1)
            {
                events = _eventRepository.GetAllZoongaEvents(query.IsFeel).ToList();
                eventList = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Event>>(events).ToList();
            }
            else
            {
                assignedEvents = _eventsUserMappingRepository.GetByUserId(userDetails.Id).ToList();
                for (int k = 0; k < assignedEvents.Count; k = k + 2000)
                {
                    var eventBatcher = assignedEvents.Skip(k).Take(2000);
                    events = _eventRepository.GetByAllEventIds(eventBatcher.Select(s => s.EventId)).Distinct().ToList();
                    eventList = eventList.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Event>>(events)).ToList();
                }
            }
            return new ReportEventsQueryResult
            {
                Events = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.Event>>(eventList)
            };
        }
    }
}