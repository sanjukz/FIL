using FIL.Api.Repositories;
using FIL.Contracts.Queries.TicketCategoryTypes;
using FIL.Contracts.QueryResults.TicketCategoryTypes;
using System.Linq;

namespace FIL.Api.QueryHandlers.Payment
{
    public class TicketCategoryTypesQueryHandler : IQueryHandler<TicketCategoryTypesQuery, TicketCategoryTypesQueryResult>
    {
        private readonly ITicketCategoryTypesRepository _ticketCategoryTypesRepository;
        private readonly ITicketCategorySubTypesRepository _ticketCategorySubTypesRepository;

        public TicketCategoryTypesQueryHandler(
            ITicketCategoryTypesRepository ticketCategoryTypesRepository,
            ITicketCategorySubTypesRepository ticketCategorySubTypesRepository)
        {
            _ticketCategoryTypesRepository = ticketCategoryTypesRepository;
            _ticketCategorySubTypesRepository = ticketCategorySubTypesRepository;
        }

        public TicketCategoryTypesQueryResult Handle(TicketCategoryTypesQuery query)
        {
            var ticketCategoryType = _ticketCategoryTypesRepository.GetAll();
            var ticketCategorySubType = _ticketCategorySubTypesRepository.GetAll();

            return new TicketCategoryTypesQueryResult
            {
                TicketCategoryTypes = ticketCategoryType.ToList(),
                TicketCategorySubTypes = ticketCategorySubType.ToList()
            };
        }
    }
}