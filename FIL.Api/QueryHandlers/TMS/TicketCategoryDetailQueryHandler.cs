using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResults.TMS;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS
{
    public class TicketCategoryDetailQueryHandler : IQueryHandler<TicketCategoryDetailQuery, TicketCategoryDetailQueryResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IEventSponsorMappingRepository _eventSponsorMappingRepository;

        public TicketCategoryDetailQueryHandler(IEventDetailRepository eventDetailRepository, ISponsorRepository sponsorRepository, IEventSponsorMappingRepository eventSponsorMappingRepository)
        {
            _sponsorRepository = sponsorRepository;
            _eventSponsorMappingRepository = eventSponsorMappingRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        public TicketCategoryDetailQueryResult Handle(TicketCategoryDetailQuery query)
        {
            var eventDetails = _eventDetailRepository.GetByAltId(query.EventDetailAltId);
            var eventSponsorList = _eventSponsorMappingRepository.GetByEventDetailId(eventDetails.Id);
            var SponsorList = AutoMapper.Mapper.Map<List<Sponsor>>(_sponsorRepository.GetByIds(eventSponsorList.Select(s => s.Id)).Where(w => w.IsEnabled));
            return new TicketCategoryDetailQueryResult
            {
                sponsors = SponsorList
            };
        }
    }
}