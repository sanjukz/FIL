using FIL.Api.Repositories;
using FIL.Contracts.Queries.Reporting.Graph;
using FIL.Contracts.QueryResults.Reporting.Graph;
using System;

namespace FIL.Api.QueryHandlers.TransactionReport.Graph
{
    public class GetMultiSubEventQueryHandler : IQueryHandler<GetMultiSubeventQuery, GetMultiSubeventQueryResult>
    {
        private readonly IReportingRepository _reportingRepository;

        public GetMultiSubEventQueryHandler(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }

        public GetMultiSubeventQueryResult Handle(GetMultiSubeventQuery query)
        {
            try
            {
                var eventDetails = _reportingRepository.GetSubeventByVenues(query.EventAltIds, query.VenueAltIds);
                return new GetMultiSubeventQueryResult
                {
                    EventDetails = eventDetails
                };
            }
            catch (Exception e)
            {
                return new GetMultiSubeventQueryResult
                {
                    EventDetails = null
                };
            }
        }
    }
}