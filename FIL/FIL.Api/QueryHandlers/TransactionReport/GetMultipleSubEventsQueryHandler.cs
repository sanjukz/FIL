using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.TransactionReport;
using FIL.Contracts.QueryResults.TransactionReport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TransactionReport
{
    public class GetMultipleSubEventsQueryHandler : IQueryHandler<GetMultipleSubEventsQuery, GetMultipleSubEventsQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventsUserMappingRepository _eventsUserMappingRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;

        public GetMultipleSubEventsQueryHandler(
            IUserRepository userRepository,
            ICurrencyTypeRepository currencyTypeRepository,
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
            _currencyTypeRepository = currencyTypeRepository;
        }

        public GetMultipleSubEventsQueryResult Handle(GetMultipleSubEventsQuery query)
        {
            List<FIL.Contracts.Models.EventDetail> subEventList = new List<FIL.Contracts.Models.EventDetail>();
            List<EventDetail> subEventFinalList = new List<EventDetail>();
            var userDetail = _userRepository.GetByAltId(query.UserAltId);

            var eventDetail = _eventRepository.GetByAltIds(query.EventAltIds);
            var eventIds = "";
            foreach (var currentEvent in eventDetail)
            {
                eventIds = eventIds + currentEvent.Id + ",";
            }
            eventIds = eventIds.TrimEnd(',');
            var currencyDetail = _currencyTypeRepository.GetByCurrencyIdsByEventIds(eventIds);

            var assignedEvents = _eventsUserMappingRepository.GetByUserIdAndEventIds(userDetail.Id, eventDetail.Select(s => s.Id).ToList());

            List<FIL.Contracts.Models.EventDetail> subEvents = new List<FIL.Contracts.Models.EventDetail>();
            if (userDetail.RolesId != 1)
            {
                subEvents = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(_eventDetailRepository.GetByEventDetailIds(assignedEvents.Select(s => s.EventDetailId)));
            }
            else
            {
                subEvents = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(_eventDetailRepository.GetByEventIds(eventDetail.Select(s => s.Id).ToList()));
            }

            for (int k = 0; k < subEvents.Count; k = k + 2000)
            {
                var eventDetailBatcher = subEvents.Skip(k).Take(2000);
                var Guid = new Guid("E6B318DB-0945-4F96-841A-F58AED54EFCB");
                if (query.EventAltIds.Contains(Guid))
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
            return new GetMultipleSubEventsQueryResult
            {
                SubEvents = subEventFinalList,
                CurrencyTypes = currencyDetail
            };
        }
    }
}