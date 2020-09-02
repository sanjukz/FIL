using FIL.Api.Repositories;
using FIL.Contracts.Queries.TicketAlert;
using FIL.Contracts.TicketAlert;
using System.Linq;

namespace FIL.Api.QueryHandlers.TicketAlert
{
    public class TicketAlertReportQueryHandler : IQueryHandler<TicketAlertReportQuery, TicketAlertReportQueryResult>
    {
        private readonly ITicketAlertEventMappingRepository _ticketAlertEventMappingRepository;
        private readonly IEventRepository _eventRepository;

        public TicketAlertReportQueryHandler(ICountryRepository countryRepository,
            ITicketAlertEventMappingRepository ticketAlertEventMappingRepository,
            IEventDetailRepository eventDetailRepository,
            IEventRepository eventRepository)
        {
            _ticketAlertEventMappingRepository = ticketAlertEventMappingRepository;
            _eventRepository = eventRepository;
        }

        public TicketAlertReportQueryResult Handle(TicketAlertReportQuery query)
        {
            var events = _eventRepository.GetByAltId(query.AltId);
            var ticketAlertReports = _ticketAlertEventMappingRepository.GetReportDataByEventId(events.Id).ToList();

            return new TicketAlertReportQueryResult
            {
                TicketAlertReport = ticketAlertReports.ToList()
            };
        }
    }
}