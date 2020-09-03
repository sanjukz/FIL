using FIL.Api.Repositories;
using FIL.Contracts.Queries.TicketAlert;
using FIL.Contracts.QueryResults.TicketAlert;
using System.Linq;

namespace FIL.Api.QueryHandlers.Search
{
    public class TicketAlertUserMappingQueryHandler : IQueryHandler<TicketAlertUesrMappingQuery, TicketAlertUesrMappingQueryResult>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITicketAlertEventMappingRepository _ticketAlertEventMappingRepository;
        private readonly ITicketAlertUserMappingRepository _ticketAlertUserMappingRepository;

        public TicketAlertUserMappingQueryHandler(ICountryRepository countryRepository,
            ITicketAlertEventMappingRepository ticketAlertEventMappingRepository,
            ITicketAlertUserMappingRepository ticketAlertUserMappingRepository,
            IEventRepository eventRepository)
        {
            _countryRepository = countryRepository;
            _eventRepository = eventRepository;
            _ticketAlertUserMappingRepository = ticketAlertUserMappingRepository;
            _ticketAlertEventMappingRepository = ticketAlertEventMappingRepository;
        }

        public TicketAlertUesrMappingQueryResult Handle(TicketAlertUesrMappingQuery query)
        {
            bool isAlreadySignUp = false;
            foreach (var ticketAlert in query.TicketAlertEvents)
            {
                var eventData = _eventRepository.GetByAltId(query.EventAltId);
                var ticketAlertUsermap = _ticketAlertUserMappingRepository.GetByTicketAlertEventMapAndEmailId(ticketAlert, query.Email);
                if (ticketAlertUsermap.Count() > 0)
                {
                    isAlreadySignUp = true;
                    break;
                }
            }

            return new TicketAlertUesrMappingQueryResult
            {
                IsAlredySignUp = isAlreadySignUp
            };
        }
    }
}