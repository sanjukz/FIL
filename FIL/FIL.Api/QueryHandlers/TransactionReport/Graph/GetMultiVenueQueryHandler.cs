using FIL.Api.Repositories;
using FIL.Contracts.Queries.Reporting.Graph;
using FIL.Contracts.QueryResults.Reporting.Graph;
using System;

namespace FIL.Api.QueryHandlers.TransactionReport.Graph
{
    public class GetMultiVenueQueryHandler : IQueryHandler<GetMultiVenueQuery, GetMultiVenueQueryResult>
    {
        private readonly IReportingRepository _reportingRepository;

        public GetMultiVenueQueryHandler(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }

        public GetMultiVenueQueryResult Handle(GetMultiVenueQuery query)
        {
            try
            {
                var venues = _reportingRepository.GetVenuesByEvents(query.EventAltIds);
                return new GetMultiVenueQueryResult
                {
                    Venues = venues
                };
            }
            catch (Exception e)
            {
                return new GetMultiVenueQueryResult
                {
                    Venues = null
                };
            }
        }
    }
}