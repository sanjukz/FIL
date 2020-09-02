using FIL.Api.Repositories;
using FIL.Contracts.Queries.Invoice;
using FIL.Contracts.QueryResults.Invoice;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS.Invoice
{
    public class SponsorOrderDetailsQueryHandler : IQueryHandler<SponsorOrderDetailsQuery, SponsorOrderDetailsQueryResult>
    {
        private readonly ICorporateOrderRequestRepository _corporateOrderRequestRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventRepository _eventRepository;
        private readonly FIL.Logging.ILogger _logger;

        public SponsorOrderDetailsQueryHandler(ICorporateOrderRequestRepository corporateOrderRequestRepository,
            IVenueRepository venueRepository,
            IEventRepository eventRepository,
            FIL.Logging.ILogger logger)
        {
            _corporateOrderRequestRepository = corporateOrderRequestRepository;
            _venueRepository = venueRepository;
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public SponsorOrderDetailsQueryResult Handle(SponsorOrderDetailsQuery query)
        {
            try
            {
                var venues = _venueRepository.GetByAltId(query.venueAltId);
                var events = _eventRepository.GetByAltId(query.eventAltId);
                var sponserOrderData = _corporateOrderRequestRepository.GetSponsorOrderData(venues.Id, events.Id, query.sponsorId).ToList();
                return new SponsorOrderDetailsQueryResult
                {
                    SponsorOrderDetailModels = sponserOrderData
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new SponsorOrderDetailsQueryResult
                {
                    SponsorOrderDetailModels = null
                };
            }
        }
    }
}