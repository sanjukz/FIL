using FIL.Api.Repositories;
using FIL.Contracts.Queries.Reporting.Graph;
using FIL.Contracts.QueryResults.Reporting.Graph;
using System;

namespace FIL.Api.QueryHandlers.TransactionReport.Graph
{
    public class GetMultiTicketCategoryQueryHandler : IQueryHandler<GetMultiTicketCategoryQuery, GetMultiTicketCategoryQueryResult>
    {
        private readonly IReportingRepository _reportingRepository;

        public GetMultiTicketCategoryQueryHandler(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }

        public GetMultiTicketCategoryQueryResult Handle(GetMultiTicketCategoryQuery query)
        {
            try
            {
                var ticketCategories = _reportingRepository.GetTicketCategoryBySubevents(query.EventDetailIds);
                var currencyTypes = _reportingRepository.GetCurrencyBySubevents(query.EventDetailIds);
                return new GetMultiTicketCategoryQueryResult
                {
                    TicketCategories = ticketCategories,
                    CurrencyTypes = currencyTypes
                };
            }
            catch (Exception e)
            {
                return new GetMultiTicketCategoryQueryResult
                {
                    TicketCategories = null,
                    CurrencyTypes = null
                };
            }
        }
    }
}