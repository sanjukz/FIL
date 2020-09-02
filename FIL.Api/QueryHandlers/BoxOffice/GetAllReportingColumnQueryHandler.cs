using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class GetAllReportingColumnQueryHandler : IQueryHandler<GetAllReportingColumnQuery, GetAllReportingColumnQueryResult>
    {
        private readonly IReportingColumnRepository _reportingColumnRepository;

        public GetAllReportingColumnQueryHandler(IReportingColumnRepository reportingColumnRepository)
        {
            _reportingColumnRepository = reportingColumnRepository;
        }

        public GetAllReportingColumnQueryResult Handle(GetAllReportingColumnQuery query)
        {
            var reportingColumn = _reportingColumnRepository.GetAll();
            return new GetAllReportingColumnQueryResult
            {
                reportingColumns = AutoMapper.Mapper.Map<List<ReportingColumn>>(reportingColumn)
            };
        }
    }
}