using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResult.TMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS
{
    public class SponsorByMatchQueryHandler : IQueryHandler<SponsorByMatchQuery, SponsorByMatchQueryResult>
    {
        private readonly IVenueRepository _venuetRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IEventSponsorMappingRepository _eventSponsorMappingRepository;

        public SponsorByMatchQueryHandler(
            IVenueRepository venuetRepository,
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            ISponsorRepository sponsorRepository,
            IEventSponsorMappingRepository eventSponsorMappingRepository)
        {
            _venuetRepository = venuetRepository;
            _eventRepository = eventRepository;
            _sponsorRepository = sponsorRepository;
            _eventSponsorMappingRepository = eventSponsorMappingRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        public SponsorByMatchQueryResult Handle(SponsorByMatchQuery query)
        {
            List<Sponsor> SponsorList = new List<Sponsor>();
            List<FIL.Contracts.DataModels.EventSponsorMapping> eventSponsorList = new List<FIL.Contracts.DataModels.EventSponsorMapping>();
            FIL.Contracts.DataModels.Event events = new FIL.Contracts.DataModels.Event();
            FIL.Contracts.DataModels.Venue venue = new FIL.Contracts.DataModels.Venue();
            FIL.Contracts.DataModels.EventDetail eventDetail = new FIL.Contracts.DataModels.EventDetail();
            List<FIL.Contracts.DataModels.EventDetail> eventDetailList = new List<FIL.Contracts.DataModels.EventDetail>();

            if (query.AllocationType == AllocationType.Match)
            {
                eventDetail = _eventDetailRepository.GetByAltId(query.EventDetailAltId);
                eventSponsorList = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.EventSponsorMapping>>(_eventSponsorMappingRepository.GetByEventDetailId(eventDetail.Id));
                SponsorList = AutoMapper.Mapper.Map<List<Sponsor>>(_sponsorRepository.GetByIds(eventSponsorList.Select(s => s.SponsorId)).Where(w => w.IsEnabled));
            }
            if (query.AllocationType == AllocationType.Venue)
            {
                venue = _venuetRepository.GetByAltId((Guid)query.VenueAltId);
                events = _eventRepository.GetByAltId((Guid)query.EventAltId);
                eventDetailList = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.EventDetail>>(_eventDetailRepository.GetEventDetailForTMS(events.Id, venue.Id));
                SponsorList = AutoMapper.Mapper.Map<List<Sponsor>>(_sponsorRepository.GetSponsorByEventDetailIds(eventDetailList.Select(s => s.Id).Distinct()).Where(w => w.IsEnabled));
            }
            if (query.AllocationType == AllocationType.Sponsor)
            {
            }
            return new SponsorByMatchQueryResult
            {
                sponsors = SponsorList
            };
        }
    }
}