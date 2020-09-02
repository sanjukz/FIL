using FIL.Api.Repositories;
using FIL.Contracts.Queries.Invoice;
using FIL.Contracts.QueryResults.Invoice;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS.Invoice
{
    public class InvoiceDataQueryHandler : IQueryHandler<InvoiceDataQuery, InvoiceDataQueryResult>
    {
        private readonly ICorporateOrderRequestRepository _corporateOrderRequestRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventRepository _eventRepository;
        private readonly FIL.Logging.ILogger _logger;

        public InvoiceDataQueryHandler(ICorporateOrderRequestRepository corporateOrderRequestRepository,
            IVenueRepository venueRepository,
            IEventRepository eventRepository,
            FIL.Logging.ILogger logger)
        {
            _corporateOrderRequestRepository = corporateOrderRequestRepository;
            _venueRepository = venueRepository;
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public InvoiceDataQueryResult Handle(InvoiceDataQuery query)
        {
            try
            {
                var invoiceDetails = _corporateOrderRequestRepository.GetInvoiceData(query.InvoiceId).ToList();
                return new InvoiceDataQueryResult
                {
                    InvoiceDetailModels = invoiceDetails
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new InvoiceDataQueryResult
                {
                    InvoiceDetailModels = null
                };
            }
        }
    }
}