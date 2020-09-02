using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Reporting;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TransactionReport
{
    public class ReportSubEventsQueryHandler : IQueryHandler<ReportSubEventsQuery, ReportSubEventsQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventsUserMappingRepository _eventsUserMappingRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;

        public ReportSubEventsQueryHandler(
            IUserRepository userRepository,
            IEventsUserMappingRepository eventsUserMappingRepository,
            IEventDetailRepository eventDetailRepository,
            IEventRepository eventRepository,
            IVenueRepository venueRepository
        )
        {
            _userRepository = userRepository;
            _eventsUserMappingRepository = eventsUserMappingRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
        }

        public ReportSubEventsQueryResult Handle(ReportSubEventsQuery query)
        {
            List<FIL.Contracts.Models.EventDetail> subEventList = new List<FIL.Contracts.Models.EventDetail>();
            List<EventDetail> subEventFinalList = new List<EventDetail>();
            var userDetail = _userRepository.GetByAltId(query.UserAltId);
            var eventDetail = _eventRepository.GetByAltId(query.EventAltId);
            var assignedEvents = _eventsUserMappingRepository.GetByUserIdAndEventId(userDetail.Id, eventDetail.Id);
            List<FIL.Contracts.Models.EventDetail> subEvents = new List<FIL.Contracts.Models.EventDetail>();
            if (userDetail.RolesId != 1)
            {
                subEvents = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(_eventDetailRepository.GetByEventDetailIds(assignedEvents.Select(s => s.EventDetailId)));
            }
            else
            {
                subEvents = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(_eventDetailRepository.GetAllByEventId(eventDetail.Id));
            }
            for (int k = 0; k < subEvents.Count; k = k + 2000)
            {
                var eventDetailBatcher = subEvents.Skip(k).Take(2000);
                if (query.EventAltId.ToString().ToUpper() == "E6B318DB-0945-4F96-841A-F58AED54EFCB")
                {
                    var venues = _venueRepository.GetByVenueIds(eventDetailBatcher.Select(s => s.VenueId).Distinct());
                    foreach (var item in venues)
                    {
                        subEventList.Add(new FIL.Contracts.Models.EventDetail
                        {
                            Id = item.Id,
                            Name = item.Name
                        });
                    }
                }
                else
                {
                    subEventList = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(eventDetailBatcher.OrderBy(o => o.StartDateTime));
                }

                subEventFinalList = subEventFinalList.Concat(AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(subEventList)).ToList();
            }
            return new ReportSubEventsQueryResult
            {
                SubEvents = subEventFinalList
            };
        }
    }
}