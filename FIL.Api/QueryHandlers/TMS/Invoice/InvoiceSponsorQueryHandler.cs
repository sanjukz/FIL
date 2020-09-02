using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Invoice;
using FIL.Contracts.QueryResult.TMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.TMS.Invoice
{
    public class InvoiceSponsorQueryHandler : IQueryHandler<InvoiceSponsorQuery, SponsorQueryResult>
    {
        private readonly IVenueRepository _venuetRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public InvoiceSponsorQueryHandler(IVenueRepository venuetRepository,
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            ISponsorRepository sponsorRepository,
            FIL.Logging.ILogger logger)
        {
            _venuetRepository = venuetRepository;
            _eventRepository = eventRepository;
            _sponsorRepository = sponsorRepository;
            _eventDetailRepository = eventDetailRepository;
            _logger = logger;
        }
        public SponsorQueryResult Handle(InvoiceSponsorQuery query)
        {
            try
            {
                var venue = _venuetRepository.GetByAltId((Guid)query.VenueAltId);
                var events = _eventRepository.GetByAltId((Guid)query.EventAltId);
                var eventDetailList = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.EventDetail>>(_eventDetailRepository.GetEventDetailForTMS(events.Id, venue.Id));
                var SponsorList = AutoMapper.Mapper.Map<List<Sponsor>>(_sponsorRepository.GetSponsorByEventDetailIds(eventDetailList.Select(s => s.Id).Distinct()).Where(w => w.IsEnabled));
                return new SponsorQueryResult
                {
                    sponsors = SponsorList
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new SponsorQueryResult
                {
                    sponsors = null
                };
            }
        }
    }
}