using FIL.Api.Repositories;
using FIL.Contracts.Queries.TMS.Handover;
using FIL.Contracts.QueryResults.TMS.Handover;

namespace FIL.Api.QueryHandlers.TMS.Handover
{
    public class TicketHandoverQueryHandler : IQueryHandler<TicketHandoverQuery, TicketHandoverQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ISponsorRepository _sponsorRepository;

        public TicketHandoverQueryHandler(ITransactionRepository transactionRepository, ISponsorRepository sponsorRepository)
        {
            _transactionRepository = transactionRepository;
            _sponsorRepository = sponsorRepository;
        }

        public TicketHandoverQueryResult Handle(TicketHandoverQuery query)
        {
            var ticketHandOverDetails = _transactionRepository.GetTicketHandoverDetails(query.TransactionId);
            var sponsorTicketDetails = _sponsorRepository.GetSponsorTicketDetails(query.TransactionId);

            return new TicketHandoverQueryResult
            {
                ticketHandoverDetails = ticketHandOverDetails,
                sponsorTicketDetails = sponsorTicketDetails,
            };
        }
    }
}